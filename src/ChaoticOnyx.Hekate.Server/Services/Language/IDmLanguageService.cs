using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Language;

namespace ChaoticOnyx.Hekate.Server.Services.Language
{
    public interface IDmLanguageService
    {
        public Lexer Lexer { get; }

        public Preprocessor Preprocessor { get; }

        public (List<CodeIssue>, LinkedList<SyntaxToken>) Lex(ReadOnlyMemory<char> text);

        public (List<CodeIssue>, PreprocessorContext) Preprocess(LinkedList<SyntaxToken> tokens, PreprocessorContext? context = null);

        public Task<CodeFile> ParseAsync(FileInfo file, PreprocessorContext? preprocessorContext = null, CancellationToken cancellationToken = default);
    }
}
