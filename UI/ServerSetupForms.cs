using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrazyRisk.UI
{
    // Asegúrate de que SOLO exista esta clase en el archivo
    public class ServerSetupForm : Form
    {
        // Campos inicializados con null-forgiving para evitar CS8618
        private TextBox   txtPlayerName     = null!;
        private ComboBox  cmbPlayerColor    = null!;
        private TextBox   txtPort           = null!;
        private CheckBox  chkIncludeNeutral = null!;
        private Button    btnStart          = null!;
        private Button    btnCancel         = null!;
        private GameConfiguration config    = null!; // si tu clase está en este namespace

        public ServerSetupForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "CrazyRisk - Crear partida (Servidor)";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(520, 340);
            Padding = new Padding(16);

            var lblName = new Label { Text = "Nombre:", AutoSize = true, Location = new Point(30, 30) };
            txtPlayerName = new TextBox { Location = new Point(220, 26), Width = 250 };

            var lblColor = new Label { Text = "Color:", AutoSize = true, Location = new Point(30, 70) };
            cmbPlayerColor = new ComboBox { Location = new Point(220, 66), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPlayerColor.Items.AddRange(new object[] { "Rojo", "Azul", "Verde", "Amarillo", "Negro" });

            var lblPort = new Label { Text = "Puerto:", AutoSize = true, Location = new Point(30, 110) };
            txtPort = new TextBox { Location = new Point(220, 106), Width = 250, Text = "7777" };

            chkIncludeNeutral = new CheckBox { Text = "Incluir ejército neutral", Location = new Point(220, 146), AutoSize = true };

            btnStart = new Button { Text = "Iniciar servidor", Location = new Point(220, 200), Size = new Size(140, 32) };
            btnCancel = new Button { Text = "Cancelar", Location = new Point(380, 200), Size = new Size(90, 32) };

            btnStart.Click += BtnStart_Click;
            btnCancel.Click += BtnCancel_Click;

            Controls.Add(lblName);
            Controls.Add(txtPlayerName);
            Controls.Add(lblColor);
            Controls.Add(cmbPlayerColor);
            Controls.Add(lblPort);
            Controls.Add(txtPort);
            Controls.Add(chkIncludeNeutral);
            Controls.Add(btnStart);
            Controls.Add(btnCancel);

            config = new GameConfiguration();
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            // Aquí validas y dejas listo 'config' para el servidor
            // p.ej.: config.PlayerName = txtPlayerName.Text; config.Port = int.Parse(txtPort.Text); etc.
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