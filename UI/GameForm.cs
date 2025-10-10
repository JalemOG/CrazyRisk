using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CrazyRisk.Core;
using CrazyRisk.UI.Controls;

namespace CrazyRisk.UI
{
    public class GameForm : Form
    {
        private readonly bool _isServer;
        private readonly GameConfiguration _cfg;
        private readonly Game _game = new Game();
        private readonly Random _rng = new Random();

        private MapControl map = null!;
        private Panel rightPanel = null!;
        private Label lblPlayer = null!;
        private Label lblPhase  = null!;
        private Label lblHint   = null!;
        private Button btnReinforce = null!;
        private Button btnAttack    = null!;
        private Button btnMove      = null!;
        private Button btnEndTurn   = null!;
        private NumericUpDown numTroops = null!;

        private Territory? _selectedSource;
        private Territory? _selectedTarget;

        public GameForm(bool isServer, GameConfiguration cfg)
        {
            _isServer = isServer;
            _cfg = cfg;
            BuildUI();
            BootstrapGame();
            Shown += (_, __) => { map.Bind(_game); map.RefreshAll(); };
            UpdateHUD();
        }

        private void BuildUI()
        {
            Text = "CrazyRisk - Juego";
            BackColor = Color.FromArgb(24, 26, 34);
            ForeColor = Color.White;
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1200;
            Height = 750;

            rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 300,
                Padding = new Padding(14),
                BackColor = Color.FromArgb(32, 34, 44)
            };

            var title = new Label {
                Text = "Panel de Turno",
                Font = new Font(Font, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(8, 8)
            };
            lblPlayer = new Label { Text = "Jugador: -", AutoSize = true, Location = new Point(8, 42) };
            lblPhase  = new Label { Text = "Fase: -", AutoSize = true, Location = new Point(8, 66) };
            lblHint   = new Label { Text = "—", AutoSize = true, Location = new Point(8, 92), MaximumSize = new Size(280, 0) };

            var lblTroops = new Label { Text = "Tropas:", AutoSize = true, Location = new Point(8, 140) };
            numTroops = new NumericUpDown { Location = new Point(80, 136), Width = 100, Minimum = 1, Maximum = 99, Value = 1 };

            btnReinforce = new Button { Text = "Refuerzo", Location = new Point(8, 180), Width = 260, Height = 36 };
            btnAttack    = new Button { Text = "Atacar",   Location = new Point(8, 226), Width = 260, Height = 36 };
            btnMove      = new Button { Text = "Mover",    Location = new Point(8, 272), Width = 260, Height = 36 };
            btnEndTurn   = new Button { Text = "Fin de turno", Location = new Point(8, 328), Width = 260, Height = 36 };

            btnReinforce.Click += (_, __) => DoReinforce();
            btnAttack.Click    += (_, __) => OnAttackClick();
            btnMove.Click      += (_, __) => OnMoveClick();
            btnEndTurn.Click   += (_, __) => EndTurn();

            rightPanel.Controls.AddRange(new Control[] {
                title, lblPlayer, lblPhase, lblHint, lblTroops, numTroops,
                btnReinforce, btnAttack, btnMove, btnEndTurn
            });

            map = new MapControl { Dock = DockStyle.Fill, Margin = new Padding(0) };
            map.TerritoryClicked += OnTerritoryClicked;

            Controls.Add(rightPanel); // primero Right
            Controls.Add(map);        // luego Fill
        }

        private void BootstrapGame()
        {
            var meName  = string.IsNullOrWhiteSpace(_cfg.PlayerName) ? (_isServer ? "Servidor" : "Cliente") : _cfg.PlayerName;
            var meColor = _isServer ? ConsoleColor.Red : ConsoleColor.Blue;

            _game.Players.Add(new Player(meName, meColor));
            _game.Players.Add(new Player(_isServer ? "Cliente" : "Servidor",
                                         _isServer ? ConsoleColor.Blue : ConsoleColor.Red));

            // Si no hay mapa real, usa demo
            if (_game.Map.Territories.Count == 0)
                _game.Map.BuildDemoMap();

            // Usa tu propio StartGame (crea turnManager y reparte)
            _game.StartGame();
        }

        private void UpdateHUD()
        {
            var cur = _game.CurrentPlayer?.Alias ?? "-";
            lblPlayer.Text = $"Jugador: {cur}";
            lblPhase.Text  = $"Fase: {_game.State}";

            btnReinforce.Enabled = _game.State == GameState.Reinforce;
            btnAttack.Enabled    = _game.State == GameState.Attack;
            btnMove.Enabled      = _game.State == GameState.Plan;

            map.SelectedSource = _selectedSource;
            map.SelectedTarget = _selectedTarget;

            lblHint.Text = _game.State switch
            {
                GameState.Reinforce => "Selecciona un territorio propio y asigna 'Tropas' → Refuerzo.",
                GameState.Attack    => "Selecciona ORIGEN propio (≥2 tropas) y DESTINO enemigo adyacente → Atacar.",
                GameState.Plan      => "Selecciona ORIGEN propio (≥2 tropas) y DESTINO propio adyacente → Mover.",
                _ => "—"
            };
        }

        private void OnTerritoryClicked(object? sender, Territory t)
        {
            switch (_game.State)
            {
                case GameState.Reinforce:
                    if (t.Owner == _game.CurrentPlayer)
                    {
                        _selectedSource = t;
                        _selectedTarget = null;
                        UpdateHUD();
                        map.RefreshTerritory(t);
                    }
                    break;

                case GameState.Attack:
                    if (_selectedSource == null)
                    {
                        if (t.Owner == _game.CurrentPlayer && t.Troops > 1)
                            _selectedSource = t;
                    }
                    else
                    {
                        if (IsAdjacent(_selectedSource, t) && t.Owner != _game.CurrentPlayer)
                            _selectedTarget = t;
                        else if (t.Owner == _game.CurrentPlayer && t.Troops > 1)
                            _selectedSource = t; // re-selección de origen
                    }
                    UpdateHUD();
                    map.Invalidate();
                    break;

                case GameState.Plan:
                    if (_selectedSource == null)
                    {
                        if (t.Owner == _game.CurrentPlayer && t.Troops > 1)
                            _selectedSource = t;
                    }
                    else
                    {
                        if (IsAdjacent(_selectedSource, t) && t.Owner == _game.CurrentPlayer)
                            _selectedTarget = t;
                        else if (t.Owner == _game.CurrentPlayer && t.Troops > 1)
                            _selectedSource = t;
                    }
                    UpdateHUD();
                    map.Invalidate();
                    break;
            }
        }

        private bool IsAdjacent(Territory a, Territory b)
        {
            var neigh = _game.Map.GetAdjacent(a);
            var it = neigh.GetIterator();
            while (it.HasNext()) if (it.Next() == b) return true;
            return false;
        }

        // ---------- Acciones ----------
        private void DoReinforce()
        {
            if (_game.State != GameState.Reinforce || _selectedSource == null) { Beep(); return; }
            if (_selectedSource.Owner != _game.CurrentPlayer) { Beep(); return; }

            int add = (int)numTroops.Value;
            if (add <= 0) return;

            _selectedSource.AddTroops(add);
            map.RefreshTerritory(_selectedSource);
            // Avanzamos a ATACAR
            _game.SetPhase(GameState.Attack);
            _selectedTarget = null;
            UpdateHUD();
            map.RefreshAll();
        }

        private void OnAttackClick()
        {
            if (_game.State != GameState.Attack)
            {
                _game.SetPhase(GameState.Attack);
                UpdateHUD();
                return;
            }

            if (_selectedSource == null || _selectedTarget == null) { MessageBox.Show("Elige ORIGEN y DESTINO adyacente (enemigo)."); return; }
            if (_selectedSource.Owner != _game.CurrentPlayer || _selectedTarget.Owner == _game.CurrentPlayer) { Beep(); return; }
            if (!IsAdjacent(_selectedSource, _selectedTarget)) { Beep(); return; }
            if (_selectedSource.Troops <= 1) { MessageBox.Show("Necesitas al menos 2 tropas en el territorio de ORIGEN."); return; }

            ResolveAttack(_selectedSource, _selectedTarget);
            map.RefreshAll();
            UpdateHUD();
        }

        private void OnMoveClick()
        {
            if (_game.State != GameState.Plan)
            {
                _game.SetPhase(GameState.Plan);
                UpdateHUD();
                return;
            }

            if (_selectedSource == null || _selectedTarget == null) { MessageBox.Show("Elige ORIGEN y DESTINO adyacente (propio)."); return; }
            if (_selectedSource.Owner != _game.CurrentPlayer || _selectedTarget.Owner != _game.CurrentPlayer) { Beep(); return; }
            if (!IsAdjacent(_selectedSource, _selectedTarget)) { Beep(); return; }

            int units = (int)numTroops.Value;
            if (units <= 0 || units >= _selectedSource.Troops) { MessageBox.Show("Debes dejar al menos 1 tropa en el ORIGEN."); return; }

            _selectedSource.RemoveTroops(units);
            _selectedTarget.AddTroops(units);

            _selectedSource = null;
            _selectedTarget = null;
            UpdateHUD();
            map.RefreshAll();
        }

        private void EndTurn()
        {
            _selectedSource = null;
            _selectedTarget = null;
            _game.NextTurn();
            UpdateHUD();
            map.RefreshAll();
        }

        // ---------- Lógica de combate con dados ----------
        private void ResolveAttack(Territory from, Territory to)
        {
            int atkDice = Math.Min(3, from.Troops - 1);
            int defDice = Math.Min(2, to.Troops);
            if (atkDice <= 0 || defDice <= 0) return;

            int[] a = Roll(atkDice);
            int[] d = Roll(defDice);

            // Comparar ordenados desc
            Array.Sort(a); Array.Reverse(a);
            Array.Sort(d); Array.Reverse(d);

            int rounds = Math.Min(a.Length, d.Length);
            int lostA = 0, lostD = 0;
            for (int i = 0; i < rounds; i++)
            {
                if (a[i] > d[i]) lostD++;
                else lostA++;
            }

            from.RemoveTroops(lostA);
            to.RemoveTroops(lostD);

            string msg = $"Atacante: {string.Join(", ", a)}\nDefensor: {string.Join(", ", d)}\n\n" +
                         $"Bajas atacante: {lostA}\nBajas defensor: {lostD}";

            // Conquista
            if (to.Troops <= 0)
            {
                to.Owner = _game.CurrentPlayer;
                int move = Math.Max(1, Math.Min((int)numTroops.Value, from.Troops - 1));
                if (move <= 0) move = 1;
                from.RemoveTroops(move);
                to.AddTroops(move);
                msg += $"\n\n¡Conquistado! Se mueven {move} tropas a {to.Name}.";
            }

            MessageBox.Show(msg, "Resultado del combate", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Mantén fase de Ataque para posibles ataques adicionales
            _selectedTarget = null;
            if (from.Troops <= 1) _selectedSource = null;
        }

        private int[] Roll(int n)
        {
            var r = new int[n];
            for (int i = 0; i < n; i++) r[i] = _rng.Next(1, 7);
            return r;
        }

        private void Beep() { System.Media.SystemSounds.Beep.Play(); }
    }
}
