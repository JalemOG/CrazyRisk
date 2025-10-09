using System;
using System.Drawing;
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

        private MapControl map = null!;
        private Panel rightPanel = null!;
        private Label lblPlayer = null!;
        private Label lblPhase  = null!;
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

            map = new MapControl { Dock = DockStyle.Fill, Margin = new Padding(0) };
            map.TerritoryClicked += OnTerritoryClicked;

            rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 280,
                Padding = new Padding(14),
                BackColor = Color.FromArgb(32, 34, 44)
            };

            var title = new Label { Text = "Panel de Turno", Font = new Font(Font, FontStyle.Bold), AutoSize = true };
            lblPlayer = new Label { Text = "Jugador: -", AutoSize = true, Top = 30 };
            lblPhase  = new Label { Text = "Fase: -", AutoSize = true, Top = 55 };

            var lblTroops = new Label { Text = "Tropas:", AutoSize = true, Top = 90 };
            numTroops = new NumericUpDown { Top = 110, Width = 100, Minimum = 1, Maximum = 50, Value = 1 };

            btnReinforce = new Button { Text = "Refuerzo", Top = 160, Width = 240, Height = 34 };
            btnAttack    = new Button { Text = "Atacar",   Top = 204, Width = 240, Height = 34 };
            btnMove      = new Button { Text = "Mover",    Top = 248, Width = 240, Height = 34 };
            btnEndTurn   = new Button { Text = "Fin de turno", Top = 302, Width = 240, Height = 34 };

            btnReinforce.Click += (_, __) => DoReinforce();
            btnAttack.Click    += (_, __) => BeginAttack();
            btnMove.Click      += (_, __) => BeginMove();
            btnEndTurn.Click   += (_, __) => EndTurn();

            rightPanel.Controls.Add(title);
            rightPanel.Controls.Add(lblPlayer);
            rightPanel.Controls.Add(lblPhase);
            rightPanel.Controls.Add(lblTroops);
            rightPanel.Controls.Add(numTroops);
            rightPanel.Controls.AddRange(new Control[] { btnReinforce, btnAttack, btnMove, btnEndTurn });

            Controls.Add(map);
            Controls.Add(rightPanel);
        }

        private void BootstrapGame()
        {
            var meName  = string.IsNullOrWhiteSpace(_cfg.PlayerName) ? (_isServer ? "Servidor" : "Cliente") : _cfg.PlayerName;
            var meColor = _isServer ? ConsoleColor.Red : ConsoleColor.Blue;

            _game.Players.Add(new Player(meName, meColor));
            _game.Players.Add(new Player(_isServer ? "Cliente" : "Servidor", _isServer ? ConsoleColor.Blue : ConsoleColor.Red));

            _game.StartGame();
            map.Bind(_game);
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
                        bool adjacent = IsAdjacent(_selectedSource, t);
                        if (adjacent && t.Owner != _game.CurrentPlayer)
                            _selectedTarget = t;
                    }
                    UpdateHUD();
                    break;

                case GameState.Plan:
                    if (_selectedSource == null)
                    {
                        if (t.Owner == _game.CurrentPlayer && t.Troops > 1)
                            _selectedSource = t;
                    }
                    else
                    {
                        bool adjacent = IsAdjacent(_selectedSource, t);
                        if (adjacent && t.Owner == _game.CurrentPlayer)
                            _selectedTarget = t;
                    }
                    UpdateHUD();
                    break;
            }
        }

        private bool IsAdjacent(Territory a, Territory b)
        {
            var neigh = _game.Map.GetAdjacent(a);
            var it = neigh.GetIterator();
            while (it.HasNext())
            {
                if (it.Next() == b) return true;
            }
            return false;
        }

        private void DoReinforce()
        {
            if (_game.State != GameState.Reinforce || _selectedSource == null) return;
            int add = (int)numTroops.Value;
            if (_selectedSource.Owner != _game.CurrentPlayer) return;

            _selectedSource.AddTroops(add);
            _game.SetPhase(GameState.Attack);   // ← CORREGIDO
            _selectedTarget = null;
            UpdateHUD();
            map.Invalidate();
        }

        private void BeginAttack()
        {
            if (_game.State != GameState.Attack)
                _game.SetPhase(GameState.Attack);  // ← CORREGIDO
            _selectedTarget = null;
            UpdateHUD();
        }

        private void BeginMove()
        {
            if (_game.State != GameState.Plan)
                _game.SetPhase(GameState.Plan);    // ← CORREGIDO
            _selectedTarget = null;
            UpdateHUD();
        }

        private void EndTurn()
        {
            _selectedSource = null;
            _selectedTarget = null;
            _game.NextTurn();
            UpdateHUD();
            map.Invalidate();
        }
    }
}
