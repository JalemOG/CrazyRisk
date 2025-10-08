using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrazyRisk.UI
{
    public class ClientConnectForm : Form
    {
        // Campos inicializados con null-forgiving para evitar CS8618
        private TextBox   txtPlayerName  = null!;
        private ComboBox  cmbPlayerColor = null!;
        private TextBox   txtServerIP    = null!;
        private TextBox   txtPort        = null!;
        private Button    btnConnect     = null!;
        private Button    btnCancel      = null!;
        private GameConfiguration config = null!; // si tu clase está en este namespace

        public ClientConnectForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "CrazyRisk - Conectarse a partida";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(480, 300);
            Padding = new Padding(16);

            var lblName = new Label { Text = "Nombre:", AutoSize = true, Location = new Point(30, 30) };
            txtPlayerName = new TextBox { Location = new Point(160, 26), Width = 250 };

            var lblColor = new Label { Text = "Color:", AutoSize = true, Location = new Point(30, 70) };
            cmbPlayerColor = new ComboBox { Location = new Point(160, 66), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPlayerColor.Items.AddRange(new object[] { "Rojo", "Azul", "Verde", "Amarillo", "Negro" });

            var lblIP = new Label { Text = "Servidor (IP):", AutoSize = true, Location = new Point(30, 110) };
            txtServerIP = new TextBox { Location = new Point(160, 106), Width = 250 };

            var lblPort = new Label { Text = "Puerto:", AutoSize = true, Location = new Point(30, 150) };
            txtPort = new TextBox { Location = new Point(160, 146), Width = 250, Text = "7777" };

            btnConnect = new Button { Text = "Conectar", Location = new Point(160, 200), Size = new Size(110, 32) };
            btnCancel  = new Button { Text = "Cancelar", Location = new Point(300, 200), Size = new Size(110, 32) };

            btnConnect.Click += BtnConnect_Click;
            btnCancel.Click += BtnCancel_Click;

            Controls.Add(lblName);
            Controls.Add(txtPlayerName);
            Controls.Add(lblColor);
            Controls.Add(cmbPlayerColor);
            Controls.Add(lblIP);
            Controls.Add(txtServerIP);
            Controls.Add(lblPort);
            Controls.Add(txtPort);
            Controls.Add(btnConnect);
            Controls.Add(btnCancel);

            config = new GameConfiguration();
        }

        private void BtnConnect_Click(object? sender, EventArgs e)
        {
            // Aquí validas y asignas a 'config' o lanzas evento para el exterior
            // p.ej.: config.PlayerName = txtPlayerName.Text; etc.
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}