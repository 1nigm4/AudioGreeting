using Microsoft.Extensions.Logging;
using System.Windows.Controls;

namespace AudioGreeting.Services.Providers
{
    public class TextBoxLoggerProvider : ILoggerProvider
    {
        private readonly TextBox _textBox;

        public TextBoxLoggerProvider(TextBox textBox)
        {
            _textBox = textBox;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TextBoxLogger(_textBox);
        }

        public void Dispose() { }
    }
}
