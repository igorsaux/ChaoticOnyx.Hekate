using System;
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

        public async Task ParseEnvironmentAsync(FileInfo dme, CancellationToken cancellationToken = default)
        {
            Files.Clear();
            await ParseRecursiveAsync(dme, new PreprocessorContext(), cancellationToken);
        }

        public async Task ParseFileAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            int codeFileIndex     = Files.FindIndex(f => f.File.FullName == file.FullName);

            if (codeFileIndex == -1)
            {
                throw new FileNotFoundException($"Файл {file.FullName} не найден.");
            }

            int previousFileIndex = Math.Max(0, codeFileIndex - 1);

            var context = codeFileIndex == previousFileIndex
                ? new PreprocessorContext()
                : Files[previousFileIndex]
                    .PreprocessorContext;

            Files[codeFileIndex] = await _languageService.ParseAsync(file, context, cancellationToken);
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
