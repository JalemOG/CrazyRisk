using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CrazyRisk.Core;

namespace CrazyRisk.UI.Controls
{
    /// <summary>
    /// Control de mapa: dibuja territorios (nodos), adyacencias (aristas) y muestra tropas.
    /// Permite seleccionar territorios con click.
    /// </summary>
    public class MapControl : Control
    {
        private readonly Dictionary<Territory, RectangleF> _bounds = new();
        private readonly Dictionary<Territory, PointF> _pos = new();
        private Territory? _selectedSource;
        private Territory? _selectedTarget;

        public Game? Game { get; private set; }

        public Territory? SelectedSource
        {
            get => _selectedSource;
            set { _selectedSource = value; Invalidate(); }
        }
        public Territory? SelectedTarget
        {
            get => _selectedTarget;
            set { _selectedTarget = value; Invalidate(); }
        }

        public event EventHandler<Territory>? TerritoryClicked;

        public MapControl()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        public void Bind(Game game)
        {
            Game = game;
            BuildAutoLayout();
            Invalidate();
        }

        /// <summary>
        /// Permite setear un layout externo (por ejemplo desde JSON),
        /// donde cada territorio tiene un PointF normalizado [0..1].
        /// </summary>
        public void SetLayout(Dictionary<string, PointF> normalizedByName)
        {
            if (Game == null) return;
            _pos.Clear();
            foreach (var t in GetAllTerritories())
            {
                if (normalizedByName.TryGetValue(t.Name, out var p))
                    _pos[t] = new PointF(
                        40 + p.X * (Width - 80),
                        40 + p.Y * (Height - 80)
                    );
            }
            BuildNodeBounds();
            Invalidate();
        }

        private IEnumerable<Territory> GetAllTerritories()
        {
            if (Game == null) yield break;
            var it = Game.Map.Territories.GetIterator();
            while (it.HasNext()) yield return it.Next();
        }

        private IEnumerable<Territory> GetNeighbors(Territory t)
        {
            var list = Game!.Map.GetAdjacent(t);
            var it = list.GetIterator();
            while (it.HasNext()) yield return it.Next();
        }

        private void BuildAutoLayout()
        {
            _pos.Clear();

            if (Game == null) return;

            // Intento 1: por continente (si existe)
            var continentsProp = Game.Map.Continents; // si no tienes Continents, comenta este bloque y se usa el grid
            if (continentsProp != null)
            {
                // Columna por continente, filas por territorio
                int col = 0;
                float margin = 40f;
                float colWidth = Math.Max(160f, (Width - margin * 2) / 6f);
                float rowHeight = 80f;

                var cit = continentsProp.GetIterator();
                while (cit.HasNext())
                {
                    var cont = cit.Next();
                    int row = 0;
                    var tit = cont.Territories.GetIterator();
                    while (tit.HasNext())
                    {
                        var t = tit.Next();
                        var x = margin + col * colWidth + colWidth * 0.5f;
                        var y = margin + row * rowHeight + rowHeight * 0.5f;
                        _pos[t] = new PointF(x, y);
                        row++;
                    }
                    col++;
                }
            }

            // Intento 2: grid simple si no hay continentes o están vacíos
            if (_pos.Count == 0)
            {
                int cols = 7;
                int idx = 0;
                float margin = 40f;
                float cw = (Width - margin * 2) / cols;
                float rh = 80f;

                foreach (var t in GetAllTerritories())
                {
                    int c = idx % cols;
                    int r = idx / cols;
                    _pos[t] = new PointF(margin + c * cw + cw * 0.5f, margin + r * rh + rh * 0.5f);
                    idx++;
                }
            }

            BuildNodeBounds();
        }

        private void BuildNodeBounds()
        {
            _bounds.Clear();
            float nodeSize = 36f;
            foreach (var kvp in _pos)
            {
                var p = kvp.Value;
                _bounds[kvp.Key] = new RectangleF(p.X - nodeSize / 2f, p.Y - nodeSize / 2f, nodeSize, nodeSize);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Game != null) BuildAutoLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.FromArgb(18, 20, 28));

            if (Game == null) return;

            // 1) Aristas (adyacencias)
            using (var pen = new Pen(Color.FromArgb(60, Color.White), 2))
            {
                foreach (var t in GetAllTerritories())
                {
                    var p1 = _pos[t];
                    foreach (var n in GetNeighbors(t))
                    {
                        // Para no dibujar dos veces la misma línea, solo si name menor (heurística)
                        if (string.CompareOrdinal(t.Name, n.Name) < 0 && _pos.TryGetValue(n, out var p2))
                        {
                            e.Graphics.DrawLine(pen, p1, p2);
                        }
                    }
                }
            }

            // 2) Nodos (territorios)
            foreach (var t in GetAllTerritories())
            {
                var rect = _bounds[t];
                var owner = t.Owner;
                var troops = t.Troops;

                var fill = (owner == null)
                    ? Color.DimGray
                    : ColorExtensions.FromConsoleColor(owner.Color);

                // Estados de selección
                if (t == _selectedSource) fill = ControlPaint.Light(fill, 0.25f);
                if (t == _selectedTarget) fill = ControlPaint.LightLight(fill);

                using (var br = new SolidBrush(fill))
                    e.Graphics.FillEllipse(br, rect);

                using (var pen = new Pen(Color.Black, 2))
                    e.Graphics.DrawEllipse(pen, rect);

                // Texto: nombre + tropas
                var label = $"{t.Name}\n{troops}";
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                using var textBrush = new SolidBrush(Color.White);
                e.Graphics.DrawString(label, Font, textBrush, rect, sf);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var t = HitTest(e.Location);
            if (t != null)
            {
                TerritoryClicked?.Invoke(this, t);
            }
        }

        private Territory? HitTest(Point p)
        {
            foreach (var kvp in _bounds)
            {
                if (kvp.Value.Contains(p)) return kvp.Key;
            }
            return null;
        }
    }
}
