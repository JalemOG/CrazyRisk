using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrazyRisk.UI
{
    public class MainMenuForm : Form
    {
        // Controles (null-forgiving para evitar warnings con Nullable)
        private Panel      mainPanel      = null!;
        private Label      titleLabel     = null!;
        private Button     btnCreateGame  = null!;
        private Button     btnJoinGame    = null!;
        private Button     btnExit        = null!;
        private PictureBox logoPictureBox = null!;

        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Ventana
            Text = "CrazyRisk - Menú Principal";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(600, 420);
            MinimumSize = new Size(600, 420);

            // Panel principal
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24)
            };

            // Título
            titleLabel = new Label
            {
                Text = "CrazyRisk",
                Font = new Font(FontFamily.GenericSansSerif, 28, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(220, 24)
            };

            // Logo (opcional: asigna imagen si tienes recursos)
            logoPictureBox = new PictureBox
            {
                Size = new Size(120, 120),
                Location = new Point(240, 72),
                SizeMode = PictureBoxSizeMode.Zoom
                // Image = Properties.Resources.Logo; // si tienes un recurso
            };

            // Botón: Crear partida
            btnCreateGame = new Button
            {
                Text = "Crear partida",
                Size = new Size(220, 38),
                Location = new Point(190, 220)
            };
            btnCreateGame.Click += BtnCreateGame_Click;

            // Botón: Unirse a partida
            btnJoinGame = new Button
            {
                Text = "Unirse a partida",
                Size = new Size(220, 38),
                Location = new Point(190, 266)
            };
            btnJoinGame.Click += BtnJoinGame_Click;

            // Botón: Salir
            btnExit = new Button
            {
                Text = "Salir",
                Size = new Size(220, 38),
                Location = new Point(190, 312)
            };
            btnExit.Click += BtnExit_Click;

            // Agregar controles
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(logoPictureBox);
            mainPanel.Controls.Add(btnCreateGame);
            mainPanel.Controls.Add(btnJoinGame);
            mainPanel.Controls.Add(btnExit);

            Controls.Add(mainPanel);
        }

        private void BtnCreateGame_Click(object? sender, EventArgs e)
        {
            using var form = new ServerSetupForm();
            form.ShowDialog(this);
        }

        private void BtnJoinGame_Click(object? sender, EventArgs e)
        {
            using var form = new ClientConnectForm();
            form.ShowDialog(this);
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}