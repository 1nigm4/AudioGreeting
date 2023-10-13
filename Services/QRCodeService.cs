using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using ZXing.Windows.Compatibility;

namespace AudioGreeting.Services
{
    public class QRCodeService
    {
        private readonly ILogger<QRCodeService> _logger;
        private BarcodeReader? _reader;

        public QRCodeService(ILogger<QRCodeService> logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            _reader = new BarcodeReader();
            _logger.LogInformation("Scanner initialized");
        }

        public string? Decode(Bitmap frame)
        {
            if (_reader == null) throw new ArgumentNullException();

            var result = _reader.Decode(frame);
            return result?.Text;
        }
    }
}
