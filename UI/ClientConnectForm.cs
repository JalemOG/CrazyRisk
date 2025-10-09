using System;
using System.Drawing;
using System.Windows.Forms;
using CrazyRisk.Networking;

namespace CrazyRisk.UI
{
    public class ClientConnectForm : Form
    {
        private TextBox   txtPlayerName  = null!;
        private ComboBox  cmbPlayerColor = null!;
        private TextBox   txtServerIP    = null!;
        private TextBox   txtPort        = null!;
        private Button    btnConnect     = null!;
        private Button    btnCancel      = null!;
        private Label     lblStatus      = null!;
        private GameConfiguration config = null!;

        private NetworkManager? network;

        public ClientConnectForm() { InitializeComponent(); }
        public GameConfiguration GetGameConfig() => config;

        private void InitializeComponent()
        {
            Text = "CrazyRisk - Conectarse a partida";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(520, 320);
            Padding = new Padding(16);

            var lblName = new Label { Text = "Nombre:", AutoSize = true, Location = new Point(30, 30) };
            txtPlayerName = new TextBox { Location = new Point(160, 26), Width = 300 };

            var lblColor = new Label { Text = "Color:", AutoSize = true, Location = new Point(30, 70) };
            cmbPlayerColor = new ComboBox { Location = new Point(160, 66), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPlayerColor.Items.AddRange(new object[] { "Rojo", "Azul", "Verde", "Amarillo", "Negro" });

            var lblIP = new Label { Text = "Servidor (IP):", AutoSize = true, Location = new Point(30, 110) };
            txtServerIP = new TextBox { Location = new Point(160, 106), Width = 300, Text = "127.0.0.1" };

            var lblPort = new Label { Text = "Puerto:", AutoSize = true, Location = new Point(30, 150) };
            txtPort = new TextBox { Location = new Point(160, 146), Width = 300, Text = "7777" };

            btnConnect = new Button { Text = "Conectar", Location = new Point(160, 200), Size = new Size(120, 32) };
            btnCancel  = new Button { Text = "Cancelar", Location = new Point(300, 200), Size = new Size(120, 32) };

            lblStatus = new Label { AutoSize = true, Location = new Point(160, 245), ForeColor = Color.DimGray, Text = "Estado: desconectado" };

            btnConnect.Click += BtnConnect_Click;
            btnCancel .Click  += (_, __) => { try { network?.Close(); } catch { } this.DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblName, txtPlayerName, lblColor, cmbPlayerColor, lblIP, txtServerIP, lblPort, txtPort, btnConnect, btnCancel, lblStatus });

            config = new GameConfiguration();
        }

        private void BtnConnect_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtPort.Text, out int port) || port <= 0 || port > 65535)
                {
                    MessageBox.Show("Puerto inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var host = txtServerIP.Text.Trim();
                if (string.IsNullOrWhiteSpace(host))
                {
                    MessageBox.Show("IP/host inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                network?.Close();
                network = new NetworkManager();
                network.Connect(host, port);

                if (network.IsConnected)
                {
                    lblStatus.ForeColor = Color.ForestGreen;
                    lblStatus.Text = "Estado: conectado";

                    config.PlayerName = string.IsNullOrWhiteSpace(txtPlayerName.Text) ? "Jugador" : txtPlayerName.Text;
                    config.ServerIP   = host;
                    config.Port       = port;

                    DialogResult = DialogResult.OK;   // ✅ vuelve al MainMenu
                    Close();
                }
                else
                {
                    lblStatus.ForeColor = Color.IndianRed;
                    lblStatus.Text = "Estado: no conectado";
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.IndianRed;
                lblStatus.Text = $"Estado: error - {ex.Message}";
                MessageBox.Show($"Error al conectar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
