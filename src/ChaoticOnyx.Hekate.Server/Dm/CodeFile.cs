using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChaoticOnyx.Hekate.Scaffolds;

namespace ChaoticOnyx.Hekate.Server.Dm
{
    /// <summary>
    ///     Класс для хранения кода.
    /// </summary>
    public sealed class CodeFile
    {
        private Memory<char>             _code;
        public  LinkedList<SyntaxToken>? Tokens  { get; private set; }
        public  FileInfo                 File    { get; }
        public  List<CodeIssue>?         Issues  { get; private set; }
        public  PreprocessorContext?     Context { get; private set; }

        public CodeFile(FileInfo file)
        {
            File   = file;
        }

        /// <summary>
        ///     Производит парсинг файла.
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public LinkedList<SyntaxToken> Parse(Lexer lexer)
        {
            _code = new Memory<char>(System.IO.File.ReadAllText(File.FullName)
                                           .ToCharArray());

            var scaffold   = new TextToTokensScaffold(_code, lexer);
            Tokens = scaffold.GetResult();
            Issues = lexer.Issues;

            return Tokens;
        }

        /// <summary>
        ///     Препроцессирует файл.
        /// </summary>
        /// <param name="preprocessor">Препроцессор.</param>
        /// <param name="context">Контекст препроцессора. Если не null - используется переданный контекст без Includes.</param>
        /// <returns>Контекст препроцессора на момент конца файла.</returns>
        /// <exception cref="InvalidOperationException">Файл не был распарсен.</exception>
        public PreprocessorContext Preprocess(Preprocessor preprocessor, PreprocessorContext? context = null)
        {
            if (Tokens is null)
            {
                throw new InvalidOperationException("Файл не распарсен.");
            }

            PreprocessorContext? resultContext;

            if (context is not null)
            {
                resultContext = context with
                {
                    Includes = new List<SyntaxToken>()
                };
            }
            else
            {
                resultContext = new PreprocessorContext();
            }

            Context = preprocessor.Preprocess(Tokens, resultContext);

            Issues  = Issues?.Concat(preprocessor.Issues)
                            .ToList();

            return Context;
        }
    }
}
