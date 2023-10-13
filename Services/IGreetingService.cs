using System;
using System.Threading;
using System.Threading.Tasks;

namespace AudioGreeting.Services
{
    public interface IGreetingService
    {
        event EventHandler<double>? OnPlaying;
        event EventHandler? OnPlayed;
        bool Select(string eventName);
        Task GreetAsync(CancellationToken cancellationToken);
        void StopGreeting();
    }
}
