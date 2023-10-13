using Microsoft.Extensions.Logging;
using System;
using System.Windows.Controls;

namespace AudioGreeting.Services.Providers
{
    public class TextBoxLogger : ILogger
    {
        private readonly TextBox _textBox;

        public TextBoxLogger(TextBox textBox)
        {
            _textBox = textBox;
        }

        IDisposable? ILogger.BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = formatter(state, exception);
            _textBox.Dispatcher.Invoke(() =>
            {
                _textBox.AppendText($"[{logLevel}] {message} {Environment.NewLine}");
                _textBox.ScrollToEnd();
            });
        }
    }
}
