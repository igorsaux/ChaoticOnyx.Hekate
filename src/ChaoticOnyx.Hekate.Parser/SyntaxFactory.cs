#region

using System;
using System.Globalization;
using ChaoticOnyx.Hekate.Parser.ChaoticOnyx.Tools.StyleCop;

#endregion

namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     API для создания токенов.
    /// </summary>
    public class SyntaxFactory : IDisposable
    {
        public readonly CodeStyle Style;

        private SyntaxFactory(CodeStyle style)
        {
            Style = style;
        }

        public static SyntaxFactory CreateFactory(CodeStyle style)
        {
            return new(style);
        }

        public SyntaxToken WhiteSpace(string space)
        {
            return new(SyntaxKind.WhiteSpace, space);
        }

        public SyntaxToken EndOfLine(string ending = "\n")
        {
            return new(SyntaxKind.EndOfLine, ending);
        }

        public SyntaxToken EndOfFile(string ending = "\n")
        {
            SyntaxToken token = new(SyntaxKind.EndOfFile, string.Empty);

            if (Style.LastEmptyLine)
            {
                token.WithLeads(EndOfLine(ending));
            }
            
            return token;
        }

        public SyntaxToken SingleLineComment(string text, string ending = "\n")
        {
            text = $"//{text}";

            return new SyntaxToken(SyntaxKind.SingleLineComment, text).WithTrails(EndOfLine(ending));
        }

        public SyntaxToken MultiLineComment(string text)
        {
            text = $"/*{text}*/";

            return new(SyntaxKind.MultiLineComment, text);
        }

        public SyntaxToken Identifier(string name)
        {
            return new(SyntaxKind.Identifier, name);
        }

        public SyntaxToken TextLiteral(string text)
        {
            return new(SyntaxKind.TextLiteral, $"\"{text}\"");
        }

        public SyntaxToken NumericalLiteral(int number)
        {
            return new(SyntaxKind.NumericalLiteral, number.ToString());
        }

        public SyntaxToken NumericalLiteral(float number)
        {
            return new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture));
        }

        public SyntaxToken NumericalLiteral(double number)
        {
            return new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture));
        }

        public SyntaxToken PathLiteral(string path)
        {
            return new(SyntaxKind.PathLiteral, $"'{path}'");
        }

        public SyntaxToken ForKeyword()
        {
            return new(SyntaxKind.ForKeyword, "for");
        }

        public SyntaxToken NewKeyword()
        {
            return new(SyntaxKind.NewKeyword, "new");
        }

        public SyntaxToken GlobalKeyword()
        {
            return new(SyntaxKind.GlobalKeyword, "global");
        }

        public SyntaxToken ThrowKeyword()
        {
            return new(SyntaxKind.ThrowKeyword, "throw");
        }

        public SyntaxToken CatchKeyword()
        {
            return new(SyntaxKind.CatchKeyword, "catch");
        }

        public SyntaxToken TryKeyword()
        {
            return new(SyntaxKind.TryKeyword, "try");
        }

        public SyntaxToken VarKeyword()
        {
            return new(SyntaxKind.VarKeyword, "var");
        }

        public SyntaxToken VerbKeyword()
        {
            return new(SyntaxKind.VerbKeyword, "verb");
        }

        public SyntaxToken ProcKeyword()
        {
            return new(SyntaxKind.ProcKeyword, "proc");
        }

        public SyntaxToken InKeyword()
        {
            return new(SyntaxKind.InKeyword, "in");
        }

        public SyntaxToken IfKeyword()
        {
            return new(SyntaxKind.IfKeyword, "if");
        }

        public SyntaxToken ElseKeyword()
        {
            return new(SyntaxKind.ElseKeyword, "else");
        }

        public SyntaxToken SetKeyword()
        {
            return new(SyntaxKind.SetKeyword, "set");
        }

        public SyntaxToken AsKeyword()
        {
            return new(SyntaxKind.AsKeyword, "as");
        }

        public SyntaxToken WhileKeyword()
        {
            return new(SyntaxKind.WhileKeyword, "while");
        }

        public SyntaxToken DefineDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.DefineDirective, "#define").WithTrails(EndOfLine(ending));
        }

        public SyntaxToken IncludeDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.IncludeDirective, "#include").WithTrails(EndOfLine(ending));
        }

        public SyntaxToken IfDefDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.IfDefDirective, "#ifdef").WithTrails(EndOfLine(ending));
        }

        public SyntaxToken IfNDefDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.IfNDefDirective, "#ifndef").WithTrails(EndOfLine(ending));
        }

        public SyntaxToken EndIfDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.EndIfDirective, "#endif").WithTrails(EndOfLine(ending));
        }

        public void Dispose() { }
    }

    public static class SyntaxTokenExtensions
    {
        public static SyntaxToken WithLeads(this SyntaxToken token, params SyntaxToken[] leads)
        {
            token.AddLeadTokens(leads);

            return token;
        }

        public static SyntaxToken WithTrails(this SyntaxToken token, params SyntaxToken[] trails)
        {
            token.AddTrailTokens(trails);

            return token;
        }
    }
}
