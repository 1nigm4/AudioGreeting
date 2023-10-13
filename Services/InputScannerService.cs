using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AudioGreeting.Services
{
    internal class InputScannerService : IScannerService
    {
        private readonly ILogger _logger;

        public InputScannerService(ILogger<InputScannerService> logger)
        {
            _logger = logger;
        }

        public async IAsyncEnumerable<string> ScanAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            string? data = null;
            StringBuilder barcode = new StringBuilder();
            InputManager.Current.PreProcessInput += (sender, e) =>
            {
                if (e.StagingItem.Input is not TextCompositionEventArgs textArgs) return;
                if (textArgs.RoutedEvent != TextCompositionManager.TextInputEvent) return;
                if (textArgs.Text == null) return;

                string text = textArgs.Text.ReplaceLineEndings(string.Empty);
                barcode.Append(text);

                if (text != string.Empty) return;
                data = barcode.ToString();
                barcode.Clear();
            };

            _logger.LogInformation("Scanning...");
            while (!cancellationToken.IsCancellationRequested)
            {
                if (data != null)
                {
                    _logger.LogInformation("Scanning paused");
                    yield return data;
                    data = null;
                    _logger.LogInformation("Scanning...");
                }

                await Task.Delay(500);
            }
        }
    }
}
