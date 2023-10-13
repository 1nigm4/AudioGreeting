using System.Collections.Generic;
using System.Threading;

namespace AudioGreeting.Services
{
    internal interface IScannerService
    {
        IAsyncEnumerable<string> ScanAsync(CancellationToken cancellationToken);
    }
}