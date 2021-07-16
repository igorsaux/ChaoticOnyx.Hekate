using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Language;
using ChaoticOnyx.Hekate.Server.Services.Files;

namespace ChaoticOnyx.Hekate.Server.Services.Language
{
    public sealed class DmLanguageService : IDmLanguageService
    {
        public Lexer        Lexer        { get; }
        public Preprocessor Preprocessor { get; }

        private readonly IFileProvider _fileProvider;

        public DmLanguageService(IFileProvider fileProvider)
        {
            Lexer         = new Lexer();
            Preprocessor  = new Preprocessor();
            _fileProvider = fileProvider;
        }

        public (List<CodeIssue>, LinkedList<SyntaxToken>) Lex(ReadOnlyMemory<char> text) => Lexer.Parse(text);

        public (List<CodeIssue>, PreprocessorContext) Preprocess(LinkedList<SyntaxToken> tokens, PreprocessorContext? context = null)
        {
            if (context == null)
            {
                context = new PreprocessorContext();
            }
            else
            {
                context = context with
                {
                    Ifs = new Stack<SyntaxToken>(), Includes = new List<SyntaxToken>()
                };
            }

            return Preprocessor.Preprocess(tokens, context);
        }

        public async Task<CodeFile> ParseAsync(FileInfo file, PreprocessorContext? preprocessorContext = null, CancellationToken cancellationToken = default)
        {
            var text = await _fileProvider.ReadAsync(file, cancellationToken);
            var (lexerIssues, tokens)         = Lex(text);
            cancellationToken.ThrowIfCancellationRequested();
            var (preprocessorIssues, context) = Preprocess(tokens, preprocessorContext);

            return new CodeFile(file, tokens, lexerIssues, preprocessorIssues, context);
        }
    }
}
