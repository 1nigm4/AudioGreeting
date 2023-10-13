using AudioGreeting.Views.Windows;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Windows;

namespace AudioGreeting
{
    public partial class App : Application
    {
        private readonly MainWindow _mainWindow;
        private readonly IHost _host;

        public App(IHost host, MainWindow mainWindow)
        {
            _host = host;
            _mainWindow = mainWindow;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            _mainWindow.Show();
            base.OnStartup(e);
            await _host.WaitForShutdownAsync();
            await Task.Delay(1000);
            Shutdown();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            base.OnExit(e);
        }
    }
}
