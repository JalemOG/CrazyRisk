using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using CrazyRisk.Networking;

namespace CrazyRisk.UI
{
    public class ServerSetupForm : Form
    {
        private TextBox   txtPlayerName  = null!;
        private ComboBox  cmbPlayerColor = null!;
        private TextBox   txtPort        = null!;
        private Button    btnStart       = null!;
        private Button    btnCancel      = null!;
        private Label     lblStatus      = null!;
        private GameConfiguration config = null!;

        private NetworkManager? network;

        public ServerSetupForm() { InitializeComponent(); }
        public GameConfiguration GetGameConfig() => config;

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try { network?.Close(); } catch { }
            base.OnFormClosed(e);
        }

        private void InitializeComponent()
        {
            Text = "CrazyRisk - Crear partida (Servidor)";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(560, 320);
            Padding = new Padding(16);

            var lblName = new Label { Text = "Nombre:", AutoSize = true, Location = new Point(30, 30) };
            txtPlayerName = new TextBox { Location = new Point(220, 26), Width = 280 };

            var lblColor = new Label { Text = "Color:", AutoSize = true, Location = new Point(30, 70) };
            cmbPlayerColor = new ComboBox { Location = new Point(220, 66), Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPlayerColor.Items.AddRange(new object[] { "Rojo", "Azul", "Verde", "Amarillo", "Negro" });

            var lblPort = new Label { Text = "Puerto:", AutoSize = true, Location = new Point(30, 110) };
            txtPort = new TextBox { Location = new Point(220, 106), Width = 280, Text = "7777" };

            btnStart  = new Button { Text = "Iniciar servidor", Location = new Point(220, 160), Size = new Size(140, 32) };
            btnCancel = new Button { Text = "Cancelar",         Location = new Point(380, 160), Size = new Size(120, 32) };

            lblStatus = new Label { AutoSize = true, Location = new Point(220, 205), ForeColor = Color.DimGray, Text = "Estado: inactivo" };

            btnStart.Click  += BtnStart_Click;
            btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblName, txtPlayerName, lblColor, cmbPlayerColor, lblPort, txtPort, btnStart, btnCancel, lblStatus });

            config = new GameConfiguration();
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtPort.Text, out int port) || port <= 0 || port > 65535)
                {
                    MessageBox.Show("Puerto inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                network?.Close();
                network = new NetworkManager();
                network.StartServer(port, IPAddress.Any);          // escucha

                lblStatus.ForeColor = Color.ForestGreen;
                lblStatus.Text = $"Escuchando en {network.LocalEndpoint}";

                config.PlayerName = string.IsNullOrWhiteSpace(txtPlayerName.Text) ? "Servidor" : txtPlayerName.Text;
                config.ServerIP   = "0.0.0.0";
                config.Port       = port;
                // Si tienes color en GameConfiguration -> asigna aquí

                DialogResult = DialogResult.OK;  // ✅ deja que MainMenu abra GameForm
                Close();
            }
            catch (SocketException se) when (se.SocketErrorCode == SocketError.AddressAlreadyInUse)
            {
                lblStatus.ForeColor = Color.IndianRed;
                lblStatus.Text = "Puerto en uso";
                MessageBox.Show("Ese puerto ya está en uso por otro proceso.", "Puerto ocupado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.IndianRed;
                lblStatus.Text = $"Error: {ex.Message}";
                MessageBox.Show($"Error al iniciar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
