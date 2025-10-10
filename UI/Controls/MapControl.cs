using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CrazyRisk.Core;

namespace CrazyRisk.UI.Controls
{
    /// Control de mapa: dibuja territorios (nodos), adyacencias y tropas.
    public class MapControl : Control
    {
        private readonly Dictionary<Territory, RectangleF> _bounds = new();
        private readonly Dictionary<Territory, PointF> _pos = new();

        private Territory? _hover;

        public Game? Game { get; private set; }
        public Territory? SelectedSource { get; set; }
        public Territory? SelectedTarget { get; set; }

        public event EventHandler<Territory>? TerritoryClicked;

        public MapControl()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.FromArgb(20, 22, 30);
            ForeColor = Color.White;
            Cursor = Cursors.Hand;
        }

        public void Bind(Game game)
        {
            Game = game;
            BuildAutoLayoutGrid();   // auto-fit
            Invalidate();
        }

        public void RefreshAll() { BuildAutoLayoutGrid(); Invalidate(); }
        public void RefreshTerritory(Territory t)
        {
            if (_bounds.TryGetValue(t, out var r)) Invalidate(Rectangle.Ceiling(r));
            else Invalidate();
        }

        private IEnumerable<Territory> AllTerritories()
        {
            if (Game == null) yield break;
            var it = Game.Map.Territories.GetIterator();
            while (it.HasNext()) yield return it.Next();
        }

        private IEnumerable<Territory> Neighbors(Territory t)
        {
            var list = Game!.Map.GetAdjacent(t);
            var it = list.GetIterator();
            while (it.HasNext()) yield return it.Next();
        }

        /// <summary>
        /// Distribuye en rejilla dinámica calculando columnas/filas para que TODO quepa en el rectángulo visible.
        /// </summary>
        private void BuildAutoLayoutGrid()
        {
            _pos.Clear();
            _bounds.Clear();
            if (Game == null) return;

            // 1) Recolectar territorios
            var list = new List<Territory>();
            foreach (var t in AllTerritories()) list.Add(t);
            if (list.Count == 0) return;

            // 2) Área interna disponible
            float margin = 28f;
            float W = Math.Max(200f, Width  - margin * 2);
            float H = Math.Max(160f, Height - margin * 2);

            // 3) Calcular columnas/filas para encajar todo (grid casi cuadrado ajustado al aspecto)
            double aspect = W / H;
            int n = list.Count;
            int cols = Math.Max(1, (int)Math.Ceiling(Math.Sqrt(n * aspect)));
            int rows = Math.Max(1, (int)Math.Ceiling((double)n / cols));

            // 4) Tamaños de celda y nodo (nodo = 60% del menor lado de celda, con límites)
            float cw = W / cols;
            float rh = H / rows;
            float node = Math.Min(cw, rh) * 0.60f;
            node = Math.Max(28f, Math.Min(node, 58f)); // límites seguros

            // 5) Posicionar en rejilla
            for (int i = 0; i < n; i++)
            {
                int c = i % cols;
                int r = i / cols;
                float x = margin + c * cw + cw * 0.5f;
                float y = margin + r * rh + rh * 0.5f;
                _pos[list[i]] = new PointF(x, y);
            }

            // 6) Bounds por nodo
            foreach (KeyValuePair<Territory, PointF> kv in _pos)
            {
                var terr = kv.Key;
                var p = kv.Value;
                _bounds[terr] = new RectangleF(p.X - node / 2f, p.Y - node / 2f, node, node);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Game != null) { BuildAutoLayoutGrid(); Invalidate(); }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Territory? newHover = HitTest(e.Location);
            if (!ReferenceEquals(newHover, _hover))
            {
                var old = _hover;
                _hover = newHover;
                if (old != null && _bounds.TryGetValue(old, out var ro)) Invalidate(Rectangle.Ceiling(ro));
                if (_hover != null && _bounds.TryGetValue(_hover, out var rn)) Invalidate(Rectangle.Ceiling(rn));
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var terr = HitTest(e.Location);
            if (terr != null) TerritoryClicked?.Invoke(this, terr);
        }

        private Territory? HitTest(Point p)
        {
            foreach (var kv in _bounds)
                if (kv.Value.Contains(p)) return kv.Key;
            return null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(BackColor);

            if (Game == null) return;

            // 1) Aristas (adyacencias)
            using (var pen = new Pen(Color.FromArgb(80, 180, 200, 220), 2))
            {
                foreach (var t in AllTerritories())
                {
                    if (!_pos.TryGetValue(t, out var p1)) continue;
                    foreach (var n in Neighbors(t))
                    {
                        // Dibuja una sola vez por par
                        if (string.CompareOrdinal(t.Name, n.Name) < 0 && _pos.TryGetValue(n, out var p2))
                            e.Graphics.DrawLine(pen, p1, p2);
                    }
                }
            }

            // 2) Nodos (territorios)
            foreach (var t in AllTerritories())
            {
                var rect = _bounds[t];
                var owner = t.Owner;
                var fill = owner == null ? Color.DimGray : FromConsoleColor(owner.Color);

                // Estilos de estado
                if (t == SelectedSource) fill = ControlPaint.Light(fill, 0.25f);
                if (t == SelectedTarget) fill = ControlPaint.LightLight(fill);

                // Halo hover
                if (t == _hover)
                {
                    using var halo = new SolidBrush(Color.FromArgb(60, Color.White));
                    var haloRect = RectangleF.Inflate(rect, 6, 6);
                    e.Graphics.FillEllipse(halo, haloRect);
                }

                // Relleno y borde
                using (var br = new SolidBrush(fill)) e.Graphics.FillEllipse(br, rect);
                using (var pen = new Pen(Color.FromArgb(30, 30, 36), 2.5f)) e.Graphics.DrawEllipse(pen, rect);

                // Texto (tamaño de fuente adaptado al nodo)
                float fs = Math.Max(8f, rect.Width / 4.2f);
                using var nameFont = new Font(Font.FontFamily, fs, FontStyle.Bold);
                using var tb = new SolidBrush(Color.White);
                var label = $"{t.Name}\n{t.Troops}";
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(label, nameFont, tb, rect, sf);
            }
        }

        private static Color FromConsoleColor(ConsoleColor cc) => cc switch
        {
            ConsoleColor.Black => Color.Black,
            ConsoleColor.DarkBlue => Color.MidnightBlue,
            ConsoleColor.DarkGreen => Color.SeaGreen,
            ConsoleColor.DarkCyan => Color.Teal,
            ConsoleColor.DarkRed => Color.Firebrick,
            ConsoleColor.DarkMagenta => Color.Indigo,
            ConsoleColor.DarkYellow => Color.Olive,
            ConsoleColor.Gray => Color.Gray,
            ConsoleColor.DarkGray => Color.DimGray,
            ConsoleColor.Blue => Color.RoyalBlue,
            ConsoleColor.Green => Color.MediumSeaGreen,
            ConsoleColor.Cyan => Color.LightSeaGreen,
            ConsoleColor.Red => Color.IndianRed,
            ConsoleColor.Magenta => Color.MediumVioletRed,
            ConsoleColor.Yellow => Color.Goldenrod,
            ConsoleColor.White => Color.White,
            _ => Color.SlateGray
        };
    }
}
