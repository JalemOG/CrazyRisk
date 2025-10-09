using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrazyRisk.UI
{
    public class MainMenuForm : Form
    {
        private Panel  mainPanel  = null!;
        private Label  titleLabel = null!;
        private Button btnCreateGame = null!;
        private Button btnJoinGame   = null!;
        private Button btnExit       = null!;

        public MainMenuForm() { InitializeComponent(); }

        private void InitializeComponent()
        {
            Text = "CrazyRisk - Menú Principal";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(600, 420);

            mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24) };
            titleLabel = new Label { Text = "CrazyRisk", Font = new Font(FontFamily.GenericSansSerif, 28, FontStyle.Bold), AutoSize = true, Location = new Point(220, 24) };

            btnCreateGame = new Button { Text = "Crear partida", Size = new Size(220, 38), Location = new Point(190, 220) };
            btnJoinGame   = new Button { Text = "Unirse a partida", Size = new Size(220, 38), Location = new Point(190, 266) };
            btnExit       = new Button { Text = "Salir", Size = new Size(220, 38), Location = new Point(190, 312) };

            btnCreateGame.Click += BtnCreateGame_Click;
            btnJoinGame.Click   += BtnJoinGame_Click;
            btnExit.Click       += (_, __) => Close();

            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.AddRange(new Control[] { btnCreateGame, btnJoinGame, btnExit });
            Controls.Add(mainPanel);
        }

        private void BtnCreateGame_Click(object? sender, EventArgs e)
        {
            using var form = new ServerSetupForm();
            var result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                var cfg  = form.GetGameConfig();     // ✅ toma la config del servidor
                var game = new GameForm(true,  cfg); // host
                game.Show(this);
            }
        }

        private void BtnJoinGame_Click(object? sender, EventArgs e)
        {
            using var form = new ClientConnectForm();
            var result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                var cfg  = form.GetGameConfig();     // ✅ toma la config del cliente
                var game = new GameForm(false, cfg); // client
                game.Show(this);
            }
        }
    }
}
