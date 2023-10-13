using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AudioGreeting.Services
{
    internal class WebcamScannerService : IScannerService
    {
        private readonly ILogger _logger;
        private readonly QRCodeService _qrCodeService;
        private readonly WebcamService _webcamService;

        public WebcamScannerService(
            ILogger<WebcamScannerService> logger,
            QRCodeService qrCodeService,
            WebcamService webcamService)
        {
            _logger = logger;
            _qrCodeService = qrCodeService;
            _webcamService = webcamService;
        }

        public async IAsyncEnumerable<string> ScanAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            _qrCodeService.Initialize();
            _webcamService.Initialize();

            string? eventData = null;
            _webcamService.NewFrame += (sender, eventArgs) =>
            {
                eventData = _qrCodeService.Decode(eventArgs.Frame);
            };

            bool isStarted = await _webcamService.StartAsync();
            if (!isStarted) yield break;

            _logger.LogInformation("Scanning...");
            while (!cancellationToken.IsCancellationRequested)
            {
                if (eventData != null)
                {
                    await _webcamService.PauseAsync();
                    yield return eventData;
                    await _webcamService.StartAsync();
                    _logger.LogInformation("Scanning...");
                    eventData = null;
                }

                if (!_webcamService.IsRunning ?? false)
                    await _webcamService.TryStartAsync();

                await Task.Delay(500);
            }

            await _webcamService.StopAsync();
        }
    }
}
