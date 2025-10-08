using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrazyRisk.UI
{
    public class MainMenuForm : Form
    {
        // Campos inicializados con null-forgiving para evitar CS8618
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
            ClientSize = new Size(600, 400);

            // Controles
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            titleLabel = new Label
            {
                Text = "CrazyRisk",
                Font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(220, 20)
            };

            logoPictureBox = new PictureBox
            {
                Size = new Size(120, 120),
                Location = new Point(240, 70),
                SizeMode = PictureBoxSizeMode.Zoom,
                // Image = ... // si tienes un recurso, asígnalo aquí
            };

            btnCreateGame = new Button
            {
                Text = "Crear partida",
                Size = new Size(200, 36),
                Location = new Point(200, 210)
            };
            btnCreateGame.Click += BtnCreateGame_Click;

            btnJoinGame = new Button
            {
                Text = "Unirse a partida",
                Size = new Size(200, 36),
                Location = new Point(200, 255)
            };
            btnJoinGame.Click += BtnJoinGame_Click;

            btnExit = new Button
            {
                Text = "Salir",
                Size = new Size(200, 36),
                Location = new Point(200, 300)
            };
            btnExit.Click += BtnExit_Click;

            // Agregar al panel
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(logoPictureBox);
            mainPanel.Controls.Add(btnCreateGame);
            mainPanel.Controls.Add(btnJoinGame);
            mainPanel.Controls.Add(btnExit);

            // Agregar a la ventana
            Controls.Add(mainPanel);
        }

        private void BtnCreateGame_Click(object? sender, EventArgs e)
        {
            // Abrir formulario de configuración del servidor (si aplica)
            using var form = new ServerSetupForm();
            form.ShowDialog(this);
        }

        private void BtnJoinGame_Click(object? sender, EventArgs e)
        {
            // Abrir formulario para conectarse a un servidor (si aplica)
            using var form = new ClientConnectForm();
            form.ShowDialog(this);
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}