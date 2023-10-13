using AudioGreeting.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AudioGreeting.Services
{
    public class AudioGreetingService : IGreetingService
    {
        public event EventHandler<double>? OnPlaying;
        public event EventHandler? OnPlayed;

        private readonly ILogger _logger;
        private readonly AudioOptions _options;
        private IWavePlayer _player;
        private AudioFileReader? _audio;

        public AudioGreetingService(
            ILogger<AudioGreetingService> logger,
            IOptions<AudioOptions> options,
            IWavePlayer player)
        {
            _logger = logger;
            _options = options.Value;
            _player = player;
        }

        public bool Select(string eventName)
        {
            try
            {
                string folderPath = string.Empty;
                if (!string.IsNullOrEmpty(_options.FolderPath))
                    folderPath = Path.Combine(Environment.CurrentDirectory, _options.FolderPath);

                string fileName = eventName;
                if (!string.IsNullOrEmpty(_options.Extension))
                    fileName = eventName + "." + _options.Extension;

                string path = Path.Combine(folderPath, fileName);
                if (!File.Exists(path))
                {
                    _logger.LogWarning($"File {path} not found");
                    return false;
                }

                _audio = new AudioFileReader(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task GreetAsync(CancellationToken cancellationToken)
        {
            if (_audio == null) return;

            try
            {
                _player.Init(_audio);
                _player.Play();
                _logger.LogInformation($"Now listening: {_audio.FileName}");
                _logger.LogInformation("Press Ctrl + S to skip");

                while (!cancellationToken.IsCancellationRequested && _player.PlaybackState == PlaybackState.Playing)
                {
                    double percentage = (double)_audio.CurrentTime.Ticks / _audio.TotalTime.Ticks;
                    OnPlaying?.Invoke(this, percentage);
                    await Task.Delay(10);
                }

                _player.Stop();
                OnPlayed?.Invoke(this, null!);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        public void StopGreeting()
        {
            if (_audio == null) return;
            if (_player.PlaybackState != PlaybackState.Playing) return;
            _player.Stop();
            _logger.LogInformation("Greeting stopped");
        }
    }
}
