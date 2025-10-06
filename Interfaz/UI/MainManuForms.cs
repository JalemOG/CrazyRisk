using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrazyRisk.UI
{
    public class MainMenuForm : Form
    {
        private Panel mainPanel;
        private Label titleLabel;
        private Button btnCreateGame;
        private Button btnJoinGame;
        private Button btnExit;
        private PictureBox logoPictureBox;

        public MainMenuForm()
        {
            InitializeComponents();
            SetupLayout();
            SetupEvents();
        }

        private void InitializeComponents()
        {
            // Configuración del formulario
            this.Text = "Crazy Risk - Menú Principal";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(30, 30, 40);

            // Panel principal
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 40)
            };

            // Logo/Imagen (opcional)
            logoPictureBox = new PictureBox
            {
                Size = new Size(200, 200),
                Location = new Point(300, 50),
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.FromArgb(40, 40, 50),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Título
            titleLabel = new Label
            {
                Text = "CRAZY RISK",
                Font = new Font("Arial", 48, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 100, 100),
                AutoSize = false,
                Size = new Size(600, 80),
                Location = new Point(100, 270),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Botón Crear Partida (Servidor)
            btnCreateGame = CreateMenuButton("CREAR PARTIDA", 320);

            // Botón Unirse a Partida (Cliente)
            btnJoinGame = CreateMenuButton("UNIRSE A PARTIDA", 390);

            // Botón Salir
            btnExit = CreateMenuButton("SALIR", 460);
            btnExit.BackColor = Color.FromArgb(150, 50, 50);

            // Agregar controles al panel
            mainPanel.Controls.Add(logoPictureBox);
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(btnCreateGame);
            mainPanel.Controls.Add(btnJoinGame);
            mainPanel.Controls.Add(btnExit);

            this.Controls.Add(mainPanel);
        }

        private Button CreateMenuButton(string text, int yPosition)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(300, 50),
                Location = new Point(250, yPosition),
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 90),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 120);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(90, 90, 110);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(60, 60, 80);

            return btn;
        }

        private void SetupLayout()
        {
            // Agregar un label con información de versión
            Label versionLabel = new Label
            {
                Text = "Versión 1.0 - Proyecto CE 1103",
                Font = new Font("Arial", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(10, 540)
            };
            mainPanel.Controls.Add(versionLabel);
        }

        private void SetupEvents()
        {
            btnCreateGame.Click += BtnCreateGame_Click;
            btnJoinGame.Click += BtnJoinGame_Click;
            btnExit.Click += BtnExit_Click;
        }

        private void BtnCreateGame_Click(object sender, EventArgs e)
        {
            // Abrir formulario de configuración de servidor
            ServerSetupForm serverForm = new ServerSetupForm();
            this.Hide();

            if (serverForm.ShowDialog() == DialogResult.OK)
            {
                // Iniciar pantalla de juego como servidor
                GameForm gameForm = new GameForm(true, serverForm.GetGameConfig());
                gameForm.FormClosed += (s, args) => this.Show();
                gameForm.Show();
            }
            else
            {
                this.Show();
            }
        }

        private void BtnJoinGame_Click(object sender, EventArgs e)
        {
            // Abrir formulario de conexión a servidor
            ClientConnectForm clientForm = new ClientConnectForm();
            this.Hide();

            if (clientForm.ShowDialog() == DialogResult.OK)
            {
                // Iniciar pantalla de juego como cliente
                GameForm gameForm = new GameForm(false, clientForm.GetConnectionInfo());
                gameForm.FormClosed += (s, args) => this.Show();
                gameForm.Show();
            }
            else
            {
                this.Show();
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "¿Está seguro que desea salir?",
                "Salir",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}