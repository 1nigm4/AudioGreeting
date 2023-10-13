using AudioGreeting.Services;
using AudioGreeting.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;
using System.Windows;

namespace AudioGreeting.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddAudioGreetingService(this IServiceCollection services)
        {
            services.AddSingleton<IWavePlayer, WaveOutEvent>();
            services.AddSingleton<IGreetingService, AudioGreetingService>();
            services.AddSingleton<GreetingService>();
            return services;
        }

        public static IServiceCollection AddWebcamScannerService(this IServiceCollection services)
        {
            services.AddSingleton<QRCodeService>();
            services.AddSingleton<WebcamService>();
            services.AddSingleton<IScannerService, WebcamScannerService>();
            return services;
        }

        public static IServiceCollection AddWindows(this IServiceCollection services)
        {
            services.AddSingleton<App>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<Window, MainWindow>(services => services.GetRequiredService<MainWindow>());
            return services;
        }
    }
}
