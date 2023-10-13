using AudioGreeting.Services;
using System;
using System.Windows;

namespace AudioGreeting.Views.Windows
{
    public partial class MainWindow : Window
    {
        private readonly ConsoleWindow _consoleWindow;
        private readonly IGreetingService _greetingService;

        public MainWindow(
            ConsoleWindow consoleWindow,
            IGreetingService greetingService)
        {
            _consoleWindow = consoleWindow;
            _greetingService = greetingService;
            _greetingService.OnPlaying += GreetingServiceOnPlaying!;
            _greetingService.OnPlayed += GreetingServiceOnPlayed;
            InitializeComponent();
        }

        private void GreetingServiceOnPlayed(object? sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                ProgressBar.Visibility = Visibility.Collapsed;
            });
        }

        private void GreetingServiceOnPlaying(object sender, double e)
        {
            this.Dispatcher.Invoke(() =>
            {
                ProgressBar.Visibility = Visibility.Visible;
            });
        }

        private void OnShowConsole(object sender, RoutedEventArgs e)
        {
            _consoleWindow.Owner = this;
            _consoleWindow.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                _consoleWindow.IsParentClosed = true;
                _consoleWindow.Close();
            }
            finally
            {
                base.OnClosed(e);
            }
        }
    }
}
