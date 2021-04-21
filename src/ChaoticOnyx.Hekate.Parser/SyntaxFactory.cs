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
    public sealed class SyntaxFactory : IDisposable
    {
        public readonly CodeStyle Style;

        private SyntaxFactory(CodeStyle style) => Style = style;

        public static SyntaxFactory CreateFactory(CodeStyle style) => new(style);

        public SyntaxToken WhiteSpace(string space) => new(SyntaxKind.WhiteSpace, space);

        public SyntaxToken EndOfLine(string ending = "\n") => new(SyntaxKind.EndOfLine, ending);

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

            return new SyntaxToken(SyntaxKind.MultiLineComment, text);
        }

        public SyntaxToken Identifier(string name) => new(SyntaxKind.Identifier, name);

        public SyntaxToken TextLiteral(string text) => new(SyntaxKind.TextLiteral, $"\"{text}\"");

        public SyntaxToken NumericalLiteral(int number) => new(SyntaxKind.NumericalLiteral, number.ToString());

        public SyntaxToken NumericalLiteral(float number) => new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture));

        public SyntaxToken NumericalLiteral(double number) => new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture));

        public SyntaxToken PathLiteral(string path) => new(SyntaxKind.PathLiteral, $"'{path}'");

        public SyntaxToken ForKeyword() => new(SyntaxKind.ForKeyword, "for");

        public SyntaxToken NewKeyword() => new(SyntaxKind.NewKeyword, "new");

        public SyntaxToken GlobalKeyword() => new(SyntaxKind.GlobalKeyword, "global");

        public SyntaxToken ThrowKeyword() => new(SyntaxKind.ThrowKeyword, "throw");

        public SyntaxToken CatchKeyword() => new(SyntaxKind.CatchKeyword, "catch");

        public SyntaxToken TryKeyword() => new(SyntaxKind.TryKeyword, "try");

        public SyntaxToken VarKeyword() => new(SyntaxKind.VarKeyword, "var");

        public SyntaxToken VerbKeyword() => new(SyntaxKind.VerbKeyword, "verb");

        public SyntaxToken ProcKeyword() => new(SyntaxKind.ProcKeyword, "proc");

        public SyntaxToken InKeyword() => new(SyntaxKind.InKeyword, "in");

        public SyntaxToken IfKeyword() => new(SyntaxKind.IfKeyword, "if");

        public SyntaxToken ElseKeyword() => new(SyntaxKind.ElseKeyword, "else");

        public SyntaxToken SetKeyword() => new(SyntaxKind.SetKeyword, "set");

        public SyntaxToken AsKeyword() => new(SyntaxKind.AsKeyword, "as");

        public SyntaxToken WhileKeyword() => new(SyntaxKind.WhileKeyword, "while");

        public SyntaxToken DefineDirective() => new(SyntaxKind.DefineDirective, "#define");

        public SyntaxToken IncludeDirective() => new(SyntaxKind.IncludeDirective, "#include");

        public SyntaxToken IfDefDirective() => new(SyntaxKind.IfDefDirective, "#ifdef");

        public SyntaxToken IfNDefDirective() => new(SyntaxKind.IfNDefDirective, "#ifndef");

        public SyntaxToken EndIfDirective() => new(SyntaxKind.EndIfDirective, "#endif");

        public SyntaxToken Slash() => new(SyntaxKind.Slash, "/");

        public SyntaxToken BackwardSlashEqual() => new(SyntaxKind.BackwardSlashEqual, "\\=");

        public SyntaxToken SlashEqual() => new(SyntaxKind.SlashEqual, "/=");

        public SyntaxToken Asterisk() => new(SyntaxKind.Asterisk, "*");

        public SyntaxToken AsteriskEqual() => new(SyntaxKind.AsteriskEqual, "*=");

        public SyntaxToken DoubleAsterisk() => new(SyntaxKind.DoubleAsterisk, "**");

        public SyntaxToken Equal() => new(SyntaxKind.Equal, "=");

        public SyntaxToken DoubleEqual() => new(SyntaxKind.DoubleEqual, "==");

        public SyntaxToken ExclamationEqual() => new(SyntaxKind.ExclamationEqual, "!=");

        public SyntaxToken Exclamation() => new(SyntaxKind.Exclamation, "!");

        public SyntaxToken Greater() => new(SyntaxKind.Greater, ">");

        public SyntaxToken DoubleGreater() => new(SyntaxKind.DoubleGreater, ">>");

        public SyntaxToken DoubleGreaterEqual() => new(SyntaxKind.DoubleGreaterEqual, ">>=");

        public SyntaxToken GreaterEqual() => new(SyntaxKind.GreaterEqual, ">=");

        public SyntaxToken Lesser() => new(SyntaxKind.Lesser, "<");

        public SyntaxToken DoubleLesser() => new(SyntaxKind.DoubleLesser, "<<");

        public SyntaxToken DoubleLesserEqual() => new(SyntaxKind.DoubleLesserEqual, "<<=");

        public SyntaxToken LesserEqual() => new(SyntaxKind.LesserEqual, "<=");

        public SyntaxToken OpenParentheses() => new(SyntaxKind.OpenParenthesis, "(");

        public SyntaxToken CloseParentheses() => new(SyntaxKind.CloseParenthesis, ")");

        public SyntaxToken OpenBrace() => new(SyntaxKind.OpenBrace, "{");

        public SyntaxToken CloseBrace() => new(SyntaxKind.CloseBrace, "}");

        public SyntaxToken OpenBracket() => new(SyntaxKind.OpenBracket, "[");

        public SyntaxToken CloseBracket() => new(SyntaxKind.CloseBracket, "]");

        public SyntaxToken Plus() => new(SyntaxKind.Plus, "+");

        public SyntaxToken PlusEqual() => new(SyntaxKind.PlusEqual, "+=");

        public SyntaxToken DoublePlus() => new(SyntaxKind.DoublePlus, "++");

        public SyntaxToken Minus() => new(SyntaxKind.Minus, "-");

        public SyntaxToken MinusEqual() => new(SyntaxKind.MinusEqual, "-=");

        public SyntaxToken DoubleMinus() => new(SyntaxKind.DoubleMinus, "--");

        public SyntaxToken Comma() => new(SyntaxKind.Comma, ",");

        public SyntaxToken Percent() => new(SyntaxKind.Percent, "%");

        public SyntaxToken PercentEqual() => new(SyntaxKind.PercentEqual, "%=");

        public SyntaxToken Ampersand() => new(SyntaxKind.Ampersand, "&");

        public SyntaxToken DoubleAmpersand() => new(SyntaxKind.DoubleAmpersand, "&&");

        public SyntaxToken AmpersandEqual() => new(SyntaxKind.AmpersandEqual, "&=");

        public SyntaxToken Colon() => new(SyntaxKind.Colon, ":");

        public SyntaxToken Question() => new(SyntaxKind.Question, "?");

        public SyntaxToken Caret() => new(SyntaxKind.Caret, "^");

        public SyntaxToken CaretEqual() => new(SyntaxKind.CaretEqual, "^=");

        public SyntaxToken Bar() => new(SyntaxKind.Bar, "|");

        public SyntaxToken DoubleBar() => new(SyntaxKind.DoubleBar, "||");

        public SyntaxToken BarEqual() => new(SyntaxKind.BarEqual, "|=");

        public SyntaxToken Dot() => new(SyntaxKind.Dot, ".");

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
