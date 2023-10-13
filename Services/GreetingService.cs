using System.Threading;
using System.Threading.Tasks;

namespace AudioGreeting.Services
{
    internal class GreetingService
    {
        private readonly IGreetingService _greetingService;
        private readonly IScannerService _scannerService;

        public GreetingService(
            IGreetingService greetingService,
            IScannerService scannerService)
        {
            _greetingService = greetingService;
            _scannerService = scannerService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await foreach (string eventName in _scannerService.ScanAsync(cancellationToken))
            {
                bool isSelected = _greetingService.Select(eventName);
                if (isSelected)
                    await _greetingService.GreetAsync(cancellationToken);
            }
        }
    }
}
