using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrazyRisk.UI
{
    public class ClientConnectForm : Form
    {
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
    }
}