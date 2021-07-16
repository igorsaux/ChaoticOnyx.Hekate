using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoticOnyx.Hekate.Server.Services.Files
{
    public class CachedFileProvider : FileProvider
    {
        private readonly Dictionary<string, CachedFile> _cachedFiles = new();

        public override async Task<ReadOnlyMemory<char>> ReadAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            if (_cachedFiles.ContainsKey(file.FullName))
            {
                CachedFile? cached        = _cachedFiles[file.FullName];
                DateTime    lastWriteTime = File.GetLastWriteTime(file.FullName);

                if (cached.LastWriteTime == lastWriteTime)
                {
                    return _cachedFiles[file.FullName]
                        .Text;
                }

                ReadOnlyMemory<char> newContent = await base.ReadAsync(file, cancellationToken);
                cached.Text          = newContent;
                cached.LastWriteTime = File.GetLastWriteTime(file.FullName);

                return newContent;
            }
            else
            {
                ReadOnlyMemory<char> newContent = await base.ReadAsync(file, cancellationToken);
                _cachedFiles.Add(file.FullName, new CachedFile(File.GetLastWriteTime(file.FullName), newContent));

                return newContent;
            }
        }

        public override async Task WriteAsync(FileInfo file, ReadOnlyMemory<char> text, CancellationToken cancellationToken = default)
        {
            Task? writeTask = base.WriteAsync(file, text, cancellationToken);

            if (_cachedFiles.ContainsKey(file.FullName))
            {
                CachedFile? cached = _cachedFiles[file.FullName];
                cached.Text          = text;
                cached.LastWriteTime = DateTime.Now;
            }
            else
            {
                _cachedFiles.Add(file.FullName, new CachedFile(DateTime.Now, text));
            }

            await writeTask;
        }

        private sealed class CachedFile
        {
            public DateTime             LastWriteTime { get; set; }
            public ReadOnlyMemory<char> Text          { get; set; }

            public CachedFile(DateTime lastWriteTime, ReadOnlyMemory<char> text)
            {
                LastWriteTime = lastWriteTime;
                Text          = text;
            }
        }
    }
}
