using System.Collections.Generic;
using System.IO;

namespace ChaoticOnyx.Hekate.Server.Language
{
    public sealed record CodeFile(FileInfo File, LinkedList<SyntaxToken> Tokens, List<CodeIssue> LexerIssues, List<CodeIssue> PreprocessorIssues, PreprocessorContext PreprocessorContext);
}
