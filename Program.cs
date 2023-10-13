using AudioGreeting.Extensions;
using AudioGreeting.Services;
using AudioGreeting.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace AudioGreeting
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.Configure<AudioOptions>(builder.Configuration.GetSection("Audio"));
            builder.Services.AddWindows();
            builder.Services.AddSingleton<IScannerService, InputScannerService>();
            //builder.Services.AddWebcamScannerService();
            builder.Services.AddAudioGreetingService();
            
            builder.AddConsoleWindowLogger();
            builder.AddFileLogger();

            var app = builder.Build();

            app.AddKeyboardBindings();

            await app.RunAsync();
        }
    }
}
