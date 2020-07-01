using System.Windows;

namespace Listen2HTML
{
    // TODO:  Run in the sys tray
    // TODO:  Make an Icon, I guess?
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow main = new Listen2HTML.MainWindow();
            main.Show();
        }
    }
}
