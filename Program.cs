using System;
using System.Windows.Forms;

namespace CrazyRisk.UI
{
    static class Program
    {
        [STAThread] // Necesario para WinForms
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Lanza tu formulario principal
            Application.Run(new UI.MainMenuForm());
        }
    }
}
