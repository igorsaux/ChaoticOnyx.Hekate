using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoticOnyx.Hekate.Server.Services.Language
{
    public interface IDmEnvironmentService
    {
        public Task ParseEnvironmentAsync(FileInfo dmeFile, CancellationToken cancellationToken = default);

        public Task ParseFileAsync(FileInfo file, CancellationToken cancellationToken = default);
    }
}
