using System.Windows;

namespace Restless.App.Toolkit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Window main = new MainWindow()
            {
                Height = 760,
                Width = 1180,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                DataContext = MainWindowViewModel.Instance,
            };
            main.Show();
        }
    }
}
