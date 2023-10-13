using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AudioGreeting.Services.Providers
{
    internal class FileLogger : ILogger
    {
        string filePath;
        static object _lock = new object();

        public FileLogger(string path)
        {
            filePath = path;
        }

        IDisposable? ILogger.BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = formatter(state, exception);
            string log = $"[{DateTime.UtcNow:g}] {message}";
            lock (_lock)
            {
                File.AppendAllText(filePath, log + Environment.NewLine);
            }
        }
    }
}
