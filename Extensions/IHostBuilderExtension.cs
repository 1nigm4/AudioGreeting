using AudioGreeting.Services.Providers;
using AudioGreeting.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace AudioGreeting.Extensions
{
    public static class IHostBuilderExtension
    {
        public static HostApplicationBuilder AddConsoleWindowLogger(this HostApplicationBuilder builder)
        {
            ConsoleWindow consoleWindow = new ConsoleWindow();
            builder.Services.AddSingleton<Window>(consoleWindow);
            builder.Services.AddSingleton(consoleWindow);
            builder.Logging.AddProvider(new TextBoxLoggerProvider(consoleWindow.ConsoleTextBox));
            return builder;
        }

        public static HostApplicationBuilder AddFileLogger(this HostApplicationBuilder builder)
        {
            string? filePath = builder.Configuration.GetValue<string>("Logger:File");
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            FileLoggerProvider fileLoggerProvider = new FileLoggerProvider(filePath);
            builder.Logging.AddProvider(fileLoggerProvider);
            return builder;
        }
    }
}
