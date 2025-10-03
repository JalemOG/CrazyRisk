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
    
    // ============================================
    // FORMULARIO DE CONFIGURACIÓN DE SERVIDOR
    // ============================================
    public class ServerSetupForm : Form
    {
        private TextBox txtPlayerName;
        private ComboBox cmbPlayerColor;
        private TextBox txtPort;
        private CheckBox chkIncludeNeutral;
        private Button btnStart;
        private Button btnCancel;
        private GameConfiguration config;
        
        public ServerSetupForm()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            this.Text = "Crear Partida - Configuración";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(40, 40, 50);
            
            // Título
            Label titleLabel = new Label
            {
                Text = "CONFIGURAR SERVIDOR",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(460, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Nombre del jugador
            Label lblName = CreateLabel("Nombre del Jugador:", 80);
            txtPlayerName = CreateTextBox(110);
            txtPlayerName.Text = "Servidor";
            
            // Color del jugador
            Label lblColor = CreateLabel("Color:", 150);
            cmbPlayerColor = new ComboBox
            {
                Location = new Point(180, 150),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 12)
            };
            cmbPlayerColor.Items.AddRange(new object[] { "Rojo", "Azul", "Verde", "Amarillo", "Morado" });
            cmbPlayerColor.SelectedIndex = 0;
            
            // Puerto
            Label lblPort = CreateLabel("Puerto:", 190);
            txtPort = CreateTextBox(220);
            txtPort.Text = "5000";
            txtPort.MaxLength = 5;
            
            // Incluir ejército neutral
            chkIncludeNeutral = new CheckBox
            {
                Text = "Incluir Ejército Neutral",
                Location = new Point(50, 260),
                Size = new Size(300, 30),
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                Checked = true
            };
            
            // Botones
            btnStart = new Button
            {
                Text = "INICIAR SERVIDOR",
                Size = new Size(200, 40),
                Location = new Point(50, 310),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(70, 150, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            
            btnCancel = new Button
            {
                Text = "CANCELAR",
                Size = new Size(200, 40),
                Location = new Point(260, 310),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(150, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            
            btnStart.Click += BtnStart_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            
            // Agregar controles
            this.Controls.Add(titleLabel);
            this.Controls.Add(lblName);
            this.Controls.Add(txtPlayerName);
            this.Controls.Add(lblColor);
            this.Controls.Add(cmbPlayerColor);
            this.Controls.Add(lblPort);
            this.Controls.Add(txtPort);
            this.Controls.Add(chkIncludeNeutral);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnCancel);
        }
        
        private Label CreateLabel(string text, int yPosition)
        {
            return new Label
            {
                Text = text,
                Location = new Point(50, yPosition),
                Size = new Size(300, 25),
                Font = new Font("Arial", 12),
                ForeColor = Color.White
            };
        }
        
        private TextBox CreateTextBox(int yPosition)
        {
            return new TextBox
            {
                Location = new Point(180, yPosition),
                Size = new Size(200, 30),
                Font = new Font("Arial", 12)
            };
        }
        
        private void BtnStart_Click(object sender, EventArgs e)
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Por favor ingrese un nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (!int.TryParse(txtPort.Text, out int port) || port < 1024 || port > 65535)
            {
                MessageBox.Show("Puerto inválido (1024-65535)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Crear configuración
            config = new GameConfiguration
            {
                PlayerName = txtPlayerName.Text,
                PlayerColor = GetSelectedColor(),
                Port = port,
                IncludeNeutral = chkIncludeNeutral.Checked,
                IsServer = true
            };
            
            this.DialogResult = DialogResult.OK;
        }
        
        private ConsoleColor GetSelectedColor()
        {
            switch (cmbPlayerColor.SelectedIndex)
            {
                case 0: return ConsoleColor.Red;
                case 1: return ConsoleColor.Blue;
                case 2: return ConsoleColor.Green;
                case 3: return ConsoleColor.Yellow;
                case 4: return ConsoleColor.Magenta;
                default: return ConsoleColor.Red;
            }
        }
        
        public GameConfiguration GetGameConfig()
        {
            return config;
        }
    }
    
    // ============================================
    // FORMULARIO DE CONEXIÓN CLIENTE
    // ============================================
    public class ClientConnectForm : Form
    {
        private TextBox txtPlayerName;
        private ComboBox cmbPlayerColor;
        private TextBox txtServerIP;
        private TextBox txtPort;
        private Button btnConnect;
        private Button btnCancel;
        private GameConfiguration config;
        
        public ClientConnectForm()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            this.Text = "Unirse a Partida";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(40, 40, 50);
            
            // Título
            Label titleLabel = new Label
            {
                Text = "CONECTAR AL SERVIDOR",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(460, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Nombre del jugador
            Label lblName = CreateLabel("Nombre del Jugador:", 80);
            txtPlayerName = CreateTextBox(110);
            txtPlayerName.Text = "Cliente";
            
            // Color del jugador
            Label lblColor = CreateLabel("Color:", 150);
            cmbPlayerColor = new ComboBox
            {
                Location = new Point(180, 150),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 12)
            };
            cmbPlayerColor.Items.AddRange(new object[] { "Rojo", "Azul", "Verde", "Amarillo", "Morado" });
            cmbPlayerColor.SelectedIndex = 1;
            
            // IP del servidor
            Label lblIP = CreateLabel("IP del Servidor:", 190);
            txtServerIP = CreateTextBox(220);
            txtServerIP.Text = "127.0.0.1";
            
            // Puerto
            Label lblPort = CreateLabel("Puerto:", 260);
            txtPort = CreateTextBox(290);
            txtPort.Text = "5000";
            txtPort.MaxLength = 5;
            
            // Botones
            btnConnect = new Button
            {
                Text = "CONECTAR",
                Size = new Size(200, 40),
                Location = new Point(50, 330),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(70, 150, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            
            btnCancel = new Button
            {
                Text = "CANCELAR",
                Size = new Size(200, 40),
                Location = new Point(260, 330),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(150, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            
            btnConnect.Click += BtnConnect_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            
            // Agregar controles
            this.Controls.Add(titleLabel);
            this.Controls.Add(lblName);
            this.Controls.Add(txtPlayerName);
            this.Controls.Add(lblColor);
            this.Controls.Add(cmbPlayerColor);
            this.Controls.Add(lblIP);
            this.Controls.Add(txtServerIP);
            this.Controls.Add(lblPort);
            this.Controls.Add(txtPort);
            this.Controls.Add(btnConnect);
            this.Controls.Add(btnCancel);
        }
        
        private Label CreateLabel(string text, int yPosition)
        {
            return new Label
            {
                Text = text,
                Location = new Point(50, yPosition),
                Size = new Size(300, 25),
                Font = new Font("Arial", 12),
                ForeColor = Color.White
            };
        }
        
        private TextBox CreateTextBox(int yPosition)
        {
            return new TextBox
            {
                Location = new Point(180, yPosition),
                Size = new Size(200, 30),
                Font = new Font("Arial", 12)
            };
        }
        
        private void BtnConnect_Click(object sender, EventArgs e)
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Por favor ingrese un nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (string.IsNullOrWhiteSpace(txtServerIP.Text))
            {
                MessageBox.Show("Por favor ingrese la IP del servidor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (!int.TryParse(txtPort.Text, out int port) || port < 1024 || port > 65535)
            {
                MessageBox.Show("Puerto inválido (1024-65535)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Crear configuración
            config = new GameConfiguration
            {
                PlayerName = txtPlayerName.Text,
                PlayerColor = GetSelectedColor(),
                ServerIP = txtServerIP.Text,
                Port = port,
                IsServer = false
            };
            
            this.DialogResult = DialogResult.OK;
        }
        
        private ConsoleColor GetSelectedColor()
        {
            switch (cmbPlayerColor.SelectedIndex)
            {
                case 0: return ConsoleColor.Red;
                case 1: return ConsoleColor.Blue;
                case 2: return ConsoleColor.Green;
                case 3: return ConsoleColor.Yellow;
                case 4: return ConsoleColor.Magenta;
                default: return ConsoleColor.Blue;
            }
        }
        
        public GameConfiguration GetConnectionInfo()
        {
            return config;
        }
    }
    
    // ============================================
    // CLASE DE CONFIGURACIÓN DEL JUEGO
    // ============================================
    public class GameConfiguration
    {
        public string PlayerName { get; set; }
        public ConsoleColor PlayerColor { get; set; }
        public string ServerIP { get; set; }
        public int Port { get; set; }
        public bool IncludeNeutral { get; set; }
        public bool IsServer { get; set; }
    }
    
    // ============================================
    // PLACEHOLDER PARA GAMEFORM (próximo paso)
    // ============================================
    public class GameForm : Form
    {
        public GameForm(bool isServer, GameConfiguration config)
        {
            this.Text = "Crazy Risk - Juego";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 40);
            
            Label lblTemp = new Label
            {
                Text = isServer ? "MODO SERVIDOR" : "MODO CLIENTE",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(400, 50),
                Location = new Point(400, 300),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            this.Controls.Add(lblTemp);
        }
    }
}