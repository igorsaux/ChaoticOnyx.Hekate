using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoticOnyx.Hekate.Server.Services.Files
{
    public class FileProvider : IFileProvider
    {
        public virtual async Task<ReadOnlyMemory<char>> ReadAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            using StreamReader? stream = file.OpenText();
            string              text   = await stream.ReadToEndAsync();

            return text.AsMemory();
        }

        public virtual async Task WriteAsync(FileInfo file, ReadOnlyMemory<char> text, CancellationToken cancellationToken = default)
        {
            await using StreamWriter? stream = file.CreateText();
            await stream.WriteAsync(text, cancellationToken);
        }
    }
}
