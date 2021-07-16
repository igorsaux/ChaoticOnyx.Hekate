using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Language;

namespace ChaoticOnyx.Hekate.Server.Services.Language
{
    public interface IDmEnvironmentService
    {
        public List<CodeFile> Files { get; }
        public Task           ParseEnvironmentAsync(FileInfo dme, CancellationToken cancellationToken = default);

        public Task ParseFileAsync(FileInfo file, CancellationToken cancellationToken = default);
    }
}
