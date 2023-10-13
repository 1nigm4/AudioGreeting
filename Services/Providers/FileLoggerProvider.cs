using Microsoft.Extensions.Logging;

namespace AudioGreeting.Services.Providers
{
    public class FileLoggerProvider : ILoggerProvider
    {
        string _path;

        public FileLoggerProvider(string path)
        {
            _path = path;
        }

        public ILogger CreateLogger(string categoryName) => new FileLogger(_path);

        public void Dispose() { }
    }
}
