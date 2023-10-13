using AudioGreeting.Services;
using AudioGreeting.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AudioGreeting.Extensions
{
    internal static class IHostExtension
    {
        public static IHost AddKeyboardBindings(this IHost host)
        {
            ConsoleWindow? consoleWindow = host.Services.GetService<ConsoleWindow>();
            if (consoleWindow == null) return host;
            CommandBinding commandExit = new CommandBinding(ApplicationCommands.Copy, (s, e) => host.StopAsync(), (s, e) => e.CanExecute = true);
            consoleWindow.CommandBindings.Add(commandExit);
            consoleWindow.ConsoleTextBox.CommandBindings.Add(commandExit);

            IGreetingService? greetingService = host.Services.GetService<IGreetingService>();
            if (greetingService == null) return host;
            var skipCommand = new CommandBinding(ApplicationCommands.Save, (s, e) => greetingService.StopGreeting(), (s, e) => e.CanExecute = true);
            CommandManager.RegisterClassCommandBinding(typeof(Window), skipCommand);
            greetingService.OnPlaying += consoleWindow.ChangePlayingState!;
            return host;
        }

        public static async Task RunAsync(this IHost app)
        {
            var cts = new CancellationTokenSource();
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(() =>
            {
                var greetingService = app.Services.GetRequiredService<GreetingService>().StartAsync(cts.Token);
                var windowApp = app.Services.GetRequiredService<App>().Run();
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                cts.Cancel();
            });

            await app.RunAsync(cts.Token);
        }
    }
}
