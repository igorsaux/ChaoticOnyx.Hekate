using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoticOnyx.Hekate.Server.Services.Files
{
    public interface IFileProvider
    {
        public Task<ReadOnlyMemory<char>> ReadAsync(FileInfo file, CancellationToken cancellationToken = default);

        public Task WriteAsync(FileInfo file, ReadOnlyMemory<char> text, CancellationToken cancellationToken = default);
    }
}
