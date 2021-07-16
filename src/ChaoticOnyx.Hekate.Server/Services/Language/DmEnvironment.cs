using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Language;

namespace ChaoticOnyx.Hekate.Server.Services.Language
{
    public sealed class DmEnvironmentService : IDmEnvironmentService
    {
        public           List<CodeFile>     Files { get; } = new();
        private readonly IDmLanguageService _languageService;

        public DmEnvironmentService(IDmLanguageService languageService)
        {
            _languageService = languageService;
        }

        public async Task ParseEnvironmentAsync(FileInfo dmeFile, CancellationToken cancellationToken = default)
        {
            Files.Clear();
            await ParseRecursiveAsync(dmeFile, new PreprocessorContext(), cancellationToken);
        }

        public async Task ParseFileAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            int  from          = Files.FindIndex(f => f.File.FullName == file.FullName);
            Files.RemoveRange(from, Files.Count - from);
            var lastFile = Files.LastOrDefault();

            await ParseRecursiveAsync(file, lastFile?
                                                .PreprocessorContext
                                            ?? new PreprocessorContext(), cancellationToken);
        }

        private async Task ParseRecursiveAsync(FileInfo file, PreprocessorContext context, CancellationToken cancellationToken = default)
        {
            var parsedFile = await _languageService.ParseAsync(file, context, cancellationToken);
            Files.Add(parsedFile);

            foreach (var includeDirective in parsedFile.PreprocessorContext.Includes)
            {
                string includePath = includeDirective.Text[1..^1];
                includePath = Path.GetFullPath(includePath, file.DirectoryName!);
                await ParseRecursiveAsync(new FileInfo(includePath), parsedFile.PreprocessorContext, cancellationToken);
            }
        }
    }
}
