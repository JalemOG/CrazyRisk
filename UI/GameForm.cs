using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrazyRisk.UI
{
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