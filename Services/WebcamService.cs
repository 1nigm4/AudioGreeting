using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AudioGreeting.Services
{
    internal class WebcamService
    {
        public bool? IsRunning => _videoCaptureDevice?.IsRunning;
        private readonly ILogger<WebcamService> _logger;
        private VideoCaptureDevice? _videoCaptureDevice;

        public WebcamService(ILogger<WebcamService> logger)
        {
            _logger = logger;
        }

        public event NewFrameEventHandler NewFrame
        {
            add
            {
                if (_videoCaptureDevice == null) return;
                _videoCaptureDevice.NewFrame += value;
            }
            remove
            {
                if (_videoCaptureDevice == null) return;
                _videoCaptureDevice.NewFrame -= value;
            }
        }

        public void Initialize()
        {
            var filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (filterInfoCollection == null || filterInfoCollection.Count == 0) 
            {
                _logger.LogError("Webcam not initialized");
                return;
            }
            
            _videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            _logger.LogInformation("Webcam initialized");
        }

        public Task<bool> StartAsync()
        {
            if (_videoCaptureDevice == null) return Task.FromResult(false);

            _videoCaptureDevice.Start();
            _logger.LogInformation("Webcam started");
            return Task.FromResult(true);
        }

        public async Task TryStartAsync()
        {
            if (_videoCaptureDevice == null) return;
            while (!_videoCaptureDevice.IsRunning)
            {
                _logger.LogWarning("Try to start webcam");
                await StartAsync();
                await Task.Delay(3000);
            }
        }

        public Task PauseAsync()
        {
            if (_videoCaptureDevice == null) return Task.CompletedTask;
            _logger.LogInformation("Webcam paused");
            _videoCaptureDevice.SignalToStop();
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            if (_videoCaptureDevice == null) return Task.CompletedTask;
            _videoCaptureDevice.SignalToStop();
            _videoCaptureDevice.Stop();
            _logger.LogInformation("Webcam stopped");
            return Task.CompletedTask;
        }
    }
}
