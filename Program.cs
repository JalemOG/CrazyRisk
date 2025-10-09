using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrazyRisk
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // Manejo global de excepciones
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            try
            {
#if NET6_0_OR_GREATER
                ApplicationConfiguration.Initialize();
#else
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
#endif
                Application.Run(new CrazyRisk.UI.MainMenuForm());
            }
            catch (Exception ex)
            {
                ShowFatal(ex);
                // re-lanzamos para que quede en logs del SO si aplica
                throw;
            }
        }

        private static void OnThreadException(object? sender, ThreadExceptionEventArgs e)
        {
            ShowError("Excepción en hilo de UI", e.Exception);
        }

        private static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                ShowFatal(ex);
        }

        private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            ShowError("Excepción no observada en Task", e.Exception);
        }

        private static void ShowError(string title, Exception ex)
        {
            try
            {
                MessageBox.Show(
                    $"{ex.Message}\n\n{ex}",
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch { /* evita crash si no hay contexto UI */ }
        }

        private static void ShowFatal(Exception ex)
        {
            try
            {
                MessageBox.Show(
                    $"Se produjo un error fatal y la aplicación se cerrará.\n\n{ex.Message}\n\n{ex}",
                    "Error fatal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop
                );
            }
            catch { /* evita crash si no hay contexto UI */ }
        }
    }
}