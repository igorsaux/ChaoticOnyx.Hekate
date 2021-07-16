using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoticOnyx.Hekate.Server.Services.Files
{
    public class FileProvider : IFileProvider
    {
        public async Task<ReadOnlyMemory<char>> ReadAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            using var stream = file.OpenText();
            string   text   = await stream.ReadToEndAsync();

            return text.AsMemory();
        }

        public async Task WriteAsync(FileInfo file, ReadOnlyMemory<char> text, CancellationToken cancellationToken = default)
        {
            await using var stream = file.CreateText();
            await stream.WriteAsync(text, cancellationToken);
        }
    }
}
