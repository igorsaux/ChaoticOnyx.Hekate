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

        public new async Task<ReadOnlyMemory<char>> ReadAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            if (_cachedFiles.ContainsKey(file.FullName))
            {
                var cached        = _cachedFiles[file.FullName];
                var lastWriteTime = File.GetLastWriteTime(file.FullName);

                if (cached.LastWriteTime == lastWriteTime)
                {
                    return _cachedFiles[file.FullName]
                        .Text;
                }

                var newContent = await base.ReadAsync(file, cancellationToken);
                cached.Text          = newContent;
                cached.LastWriteTime = File.GetLastWriteTime(file.FullName);

                return newContent;
            }
            else
            {
                var newContent = await base.ReadAsync(file, cancellationToken);
                _cachedFiles.Add(file.FullName, new CachedFile(File.GetLastWriteTime(file.FullName), newContent));

                return newContent;
            }
        }

        public new async Task WriteAsync(FileInfo file, ReadOnlyMemory<char> text, CancellationToken cancellationToken = default)
        {
            var writeTask = base.WriteAsync(file, text, cancellationToken);

            if (_cachedFiles.ContainsKey(file.FullName))
            {
                var cached = _cachedFiles[file.FullName];
                cached.Text = text;
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
            public CachedFile(DateTime lastWriteTime, ReadOnlyMemory<char> text)
            {
                this.LastWriteTime = lastWriteTime;
                this.Text          = text;
            }

            public DateTime             LastWriteTime { get; set; }
            public ReadOnlyMemory<char> Text          { get; set; }
        }
    }
}
