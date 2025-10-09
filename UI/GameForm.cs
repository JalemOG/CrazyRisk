using System;
using System.Drawing;
using System.Windows.Forms;
using CrazyRisk.Core;

namespace CrazyRisk.UI
{
    public class GameForm : Form
    {
        private readonly Game _game = new Game();
        private readonly bool _isServer;
        private readonly GameConfiguration _config;

        private Label lblInfo = null!;
        private Button btnNextTurn = null!;

        public GameForm(bool isServer, GameConfiguration config)
        {
            _isServer = isServer;
            _config = config;

            this.Text = "Crazy Risk - Juego";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 40);

            lblInfo = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Regular),
                Location = new Point(20, 20),
                Text = "Inicializando..."
            };

            btnNextTurn = new Button
            {
                Text = "Siguiente turno",
                Location = new Point(20, 60),
                Size = new Size(160, 32)
            };
            btnNextTurn.Click += (_, __) =>
            {
                var p = _game.NextTurn();
                UpdateInfo();
            };

            this.Controls.Add(lblInfo);
            this.Controls.Add(btnNextTurn);

            BootstrapGame();
            UpdateInfo();
        }

        private void BootstrapGame()
        {
            // Jugador local (host o cliente)
            var me = new Player(_config.PlayerName ?? (_isServer ? "Servidor" : "Cliente"),
                                _config.PlayerColor);
            _game.Players.Add(me);

            // Nota: como aún no hay lobby sincronizado por red,
            // añadimos un oponente local "dummy" para poder repartir territorios.
            // Cuando cablees la red, remplaza esto por los jugadores reales recibidos.
            var opponentColor = _isServer ? ConsoleColor.Blue : ConsoleColor.Red;
            var opponentName  = _isServer ? "Cliente" : "Servidor";
            _game.Players.Add(new Player(opponentName, opponentColor));

            // Iniciar juego real
            _game.StartGame();
        }

        private void UpdateInfo()
        {
            var current = _game.CurrentPlayer?.Alias ?? "(sin turno)";
            lblInfo.Text =
                $"Jugadores: {_game.Players.Count} | " +
                $"Territorios: {_game.Map.Territories.Count} | " +
                $"Estado: {_game.State} | " +
                $"Turno actual: {current}";
        }
    }
}
