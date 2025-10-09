using System;
using System.Drawing;
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
        private Button    btnCancel         = null!;
        private Label     lblStatus         = null!;
        private GameConfiguration config    = null!;

        // Mantenemos la instancia viva mientras la ventana esté abierta
        private NetworkManager? network;

        public ServerSetupForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "CrazyRisk - Crear partida (Servidor)";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(560, 380);
            Padding = new Padding(16);

            var lblName = new Label { Text = "Nombre:", AutoSize = true, Location = new Point(30, 30) };
            txtPlayerName = new TextBox { Location = new Point(220, 26), Width = 280 };

            var lblColor = new Label { Text = "Color:", AutoSize = true, Location = new Point(30, 70) };
            cmbPlayerColor = new ComboBox { Location = new Point(220, 66), Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPlayerColor.Items.AddRange(new object[] { "Rojo", "Azul", "Verde", "Amarillo", "Negro" });

            var lblPort = new Label { Text = "Puerto:", AutoSize = true, Location = new Point(30, 110) };
            txtPort = new TextBox { Location = new Point(220, 106), Width = 280, Text = "7777" };

            chkIncludeNeutral = new CheckBox { Text = "Incluir ejército neutral", Location = new Point(220, 146), AutoSize = true };

            btnStart = new Button { Text = "Iniciar servidor", Location = new Point(220, 200), Size = new Size(140, 32) };
            btnCancel = new Button { Text = "Cancelar", Location = new Point(380, 200), Size = new Size(120, 32) };

            lblStatus = new Label { AutoSize = true, Location = new Point(220, 245), ForeColor = Color.DimGray, Text = "Estado: inactivo" };

            btnStart.Click += BtnStart_Click;
            btnCancel.Click += BtnCancel_Click;

            Controls.AddRange(new Control[] {
                lblName, txtPlayerName,
                lblColor, cmbPlayerColor,
                lblPort, txtPort,
                chkIncludeNeutral,
                btnStart, btnCancel,
                lblStatus
            });

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

                network = new NetworkManager();
                network.StartServer(port);

                if (network.IsListening)
                {
                    lblStatus.ForeColor = Color.ForestGreen;
                    lblStatus.Text = $"Estado: escuchando en {network.LocalEndpoint}";
                }
                else
                {
                    lblStatus.ForeColor = Color.IndianRed;
                    lblStatus.Text = "Estado: no se pudo iniciar";
                    return;
                }

                // Espera un cliente en segundo plano (hasta 60s) y actualiza UI cuando llegue
                _ = Task.Run(() =>
                {
                    if (network!.TryAcceptClient(60000))
                    {
                        try
                        {
                            Invoke(() =>
                            {
                                lblStatus.ForeColor = Color.ForestGreen;
                                lblStatus.Text = $"Cliente conectado: {network.RemoteEndpoint}";
                            });
                        }
                        catch { /* si el form ya se cerró */ }
                    }
                });

                // Rellenar config si la usas luego
                config.PlayerName = txtPlayerName.Text;
                config.ServerIP   = "0.0.0.0";
                config.Port       = port;
                config.IncludeNeutral = chkIncludeNeutral.Checked;
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.IndianRed;
                lblStatus.Text = $"Estado: error - {ex.Message}";
                MessageBox.Show($"Error al iniciar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            try { network?.Close(); } catch { /* ignore */ }
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
