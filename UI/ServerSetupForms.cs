using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrazyRisk.Networking;

namespace CrazyRisk.UI
{
    public class ServerSetupForm : Form
    {
        private TextBox   txtPlayerName     = null!;
        private ComboBox  cmbPlayerColor    = null!;
        private TextBox   txtPort           = null!;
        private CheckBox  chkIncludeNeutral = null!;
        private Button    btnStart          = null!;
        private Button    btnStop           = null!;
        private Button    btnClose          = null!;
        private Label     lblStatus         = null!;
        private GameConfiguration config    = null!;

        private NetworkManager? network;

        public ServerSetupForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try { network?.Close(); } catch { /* ignore */ }
            base.OnFormClosed(e);
        }

        private void InitializeComponent()
        {
            Text = "CrazyRisk - Crear partida (Servidor)";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(580, 400);
            Padding = new Padding(16);

            var lblName = new Label { Text = "Nombre:", AutoSize = true, Location = new Point(30, 30) };
            txtPlayerName = new TextBox { Location = new Point(220, 26), Width = 300 };

            var lblColor = new Label { Text = "Color:", AutoSize = true, Location = new Point(30, 70) };
            cmbPlayerColor = new ComboBox { Location = new Point(220, 66), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPlayerColor.Items.AddRange(new object[] { "Rojo", "Azul", "Verde", "Amarillo", "Negro" });

            var lblPort = new Label { Text = "Puerto:", AutoSize = true, Location = new Point(30, 110) };
            txtPort = new TextBox { Location = new Point(220, 106), Width = 300, Text = "7777" };

            chkIncludeNeutral = new CheckBox { Text = "Incluir ejército neutral", Location = new Point(220, 146), AutoSize = true };

            btnStart = new Button { Text = "Iniciar servidor", Location = new Point(220, 200), Size = new Size(140, 32) };
            btnStop  = new Button { Text = "Detener",         Location = new Point(370, 200), Size = new Size(120, 32), Enabled = false };
            btnClose = new Button { Text = "Cerrar",          Location = new Point(220, 240), Size = new Size(270, 32) };

            lblStatus = new Label { AutoSize = true, Location = new Point(220, 285), ForeColor = Color.DimGray, Text = "Estado: inactivo" };

            btnStart.Click += BtnStart_Click;
            btnStop.Click  += BtnStop_Click;
            btnClose.Click += BtnClose_Click;

            Controls.AddRange(new Control[] {
                lblName, txtPlayerName,
                lblColor, cmbPlayerColor,
                lblPort, txtPort,
                chkIncludeNeutral,
                btnStart, btnStop, btnClose,
                lblStatus
            });

            config = new GameConfiguration();
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtPort.Text, out int basePort) || basePort <= 0 || basePort > 65535)
                {
                    MessageBox.Show("Puerto inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Si ya está escuchando, no vuelvas a iniciar
                if (network is not null && network.IsListening)
                {
                    lblStatus.ForeColor = Color.ForestGreen;
                    lblStatus.Text = $"Ya escuchando en {network.LocalEndpoint}";
                    return;
                }

                // Si había un NetworkManager previo, ciérralo por si quedó algo atado
                network?.Close();

                network = new NetworkManager();

                // Intenta abrir ese puerto y si está ocupado, prueba los siguientes hasta +9
                int? opened = TryOpenFirstFreePort(network, basePort, 10);
                if (opened is not int p)
                {
                    lblStatus.ForeColor = Color.IndianRed;
                    lblStatus.Text = $"Puertos {basePort}-{basePort + 9} ocupados";
                    MessageBox.Show("No se pudo iniciar: puerto en uso. Prueba otro.", "Puerto ocupado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtPort.Text = p.ToString();
                lblStatus.ForeColor = Color.ForestGreen;
                lblStatus.Text = $"Escuchando en {network.LocalEndpoint}";
                btnStart.Enabled = false;
                btnStop.Enabled  = true;

                // Acepta un cliente (hasta 60s) sin bloquear la UI
                _ = Task.Run(() =>
                {
                    if (network.TryAcceptClient(60000))
                    {
                        try
                        {
                            Invoke(() =>
                            {
                                lblStatus.ForeColor = Color.ForestGreen;
                                lblStatus.Text = $"Cliente conectado: {network.RemoteEndpoint}";

                                // 🔽 lanza el juego en el servidor
                                var gameForm = new GameForm(true, config);
                                gameForm.Show(); // o ShowDialog(this) si quieres modal
                            });
                        }
                        catch { /* por si el form se cerró */ }
                    }
                });

                // Guarda config si la usas luego
                config.PlayerName    = txtPlayerName.Text;
                config.ServerIP      = "0.0.0.0";
                config.Port          = p;
                config.IncludeNeutral = chkIncludeNeutral.Checked;
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
                MessageBox.Show($"Error al iniciar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int? TryOpenFirstFreePort(NetworkManager nm, int basePort, int attempts)
        {
            for (int p = basePort; p < basePort + attempts; p++)
            {
                try
                {
                    nm.StartServer(p, IPAddress.Any);
                    return p; // éxito
                }
                catch (SocketException se) when (se.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    // intenta siguiente puerto
                }
            }
            return null; // ninguno disponible
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            try
            {
                network?.Close();
                lblStatus.ForeColor = Color.DimGray;
                lblStatus.Text = "Estado: detenido";
            }
            finally
            {
                btnStart.Enabled = true;
                btnStop.Enabled  = false;
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            try { network?.Close(); } catch { /* ignore */ }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
