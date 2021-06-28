using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChaoticOnyx.Hekate.Server.Dm
{
    public sealed class ProjectEnvironment
    {
        private Lexer          _lexer        = new();
        private Preprocessor   _preprocessor = new();
        public  List<CodeFile> Files { get; } = new();

        public ProjectEnvironment(FileInfo dme)
        {
            Files.Add(new CodeFile(dme));
        }

        public void Parse()
        {
            var dme = Files.FirstOrDefault((f) => f.File.Extension == ".dme");

            if (dme is null)
            {
                throw new FileNotFoundException("Файл среды не найден.");
            }

            Files.Clear();
            _lexer        = new Lexer();
            _preprocessor = new Preprocessor();
            ParseFile(dme, new PreprocessorContext());
        }

        private void ParseFile(CodeFile codeFile, PreprocessorContext context)
        {
            codeFile.Parse(_lexer);
            context = codeFile.Preprocess(_preprocessor, context);
            Files.Add(codeFile);

            foreach (var include in context.Includes)
            {
                string includePath = include.Text[1..^1];
                includePath = Path.GetFullPath(includePath, codeFile.File.DirectoryName!);
                var includeCodeFile = new CodeFile(new FileInfo(includePath));
                ParseFile(includeCodeFile, context);
            }
        }
    }
}
