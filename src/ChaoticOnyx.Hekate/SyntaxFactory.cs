#region

using System.Globalization;

#endregion

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     API для создания токенов.
    /// </summary>
    public sealed class SyntaxFactory
    {
        public static SyntaxToken ElseDirective = new(SyntaxKind.ElseDirective, "#else");

        public static SyntaxToken WhiteSpace(string space) => new(SyntaxKind.WhiteSpace, space);

        public static SyntaxToken EndOfLine(string ending = "\n") => new(SyntaxKind.EndOfLine, ending);

        public static SyntaxToken MultiLineComment(string text) => new(SyntaxKind.MultiLineComment, $"/*{text}*/");

        public static SyntaxToken Identifier(string name) => new(SyntaxKind.Identifier, name);

        public static SyntaxToken NumericalLiteral(float number) => new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture));

        public static SyntaxToken PathLiteral(string path) => new(SyntaxKind.PathLiteral, $"'{path}'");

        public static SyntaxToken ForKeyword() => new(SyntaxKind.ForKeyword, "for");

        public static SyntaxToken NewKeyword() => new(SyntaxKind.NewKeyword, "new");

        public static SyntaxToken GlobalKeyword() => new(SyntaxKind.GlobalKeyword, "global");

        public static SyntaxToken ThrowKeyword() => new(SyntaxKind.ThrowKeyword, "throw");

        public static SyntaxToken CatchKeyword() => new(SyntaxKind.CatchKeyword, "catch");

        public static SyntaxToken VarKeyword() => new(SyntaxKind.VarKeyword, "var");

        public static SyntaxToken VerbKeyword() => new(SyntaxKind.VerbKeyword, "verb");

        public static SyntaxToken ProcKeyword() => new(SyntaxKind.ProcKeyword, "proc");

        public static SyntaxToken InKeyword() => new(SyntaxKind.InKeyword, "in");

        public static SyntaxToken IfKeyword() => new(SyntaxKind.IfKeyword, "if");

        public static SyntaxToken ElseKeyword() => new(SyntaxKind.ElseKeyword, "else");

        public static SyntaxToken AsKeyword() => new(SyntaxKind.AsKeyword, "as");

        public static SyntaxToken WhileKeyword() => new(SyntaxKind.WhileKeyword, "while");

        public static SyntaxToken IfDefDirective() => new(SyntaxKind.IfDefDirective, "#ifdef");

        public static SyntaxToken IfNDefDirective() => new(SyntaxKind.IfNDefDirective, "#ifndef");

        public static SyntaxToken EndIfDirective() => new(SyntaxKind.EndIfDirective, "#endif");

        public static SyntaxToken Slash() => new(SyntaxKind.Slash, "/");

        public static SyntaxToken SlashEqual() => new(SyntaxKind.SlashEqual, "/=");

        public static SyntaxToken Asterisk() => new(SyntaxKind.Asterisk, "*");

        public static SyntaxToken AsteriskEqual() => new(SyntaxKind.AsteriskEqual, "*=");

        public static SyntaxToken DoubleAsterisk() => new(SyntaxKind.DoubleAsterisk, "**");

        public static SyntaxToken Equal() => new(SyntaxKind.Equal, "=");

        public static SyntaxToken DoubleEqual() => new(SyntaxKind.DoubleEqual, "==");

        public static SyntaxToken ExclamationEqual() => new(SyntaxKind.ExclamationEqual, "!=");

        public static SyntaxToken Exclamation() => new(SyntaxKind.Exclamation, "!");

        public static SyntaxToken Greater() => new(SyntaxKind.Greater, ">");

        public static SyntaxToken DoubleGreater() => new(SyntaxKind.DoubleGreater, ">>");

        public static SyntaxToken DoubleGreaterEqual() => new(SyntaxKind.DoubleGreaterEqual, ">>=");

        public static SyntaxToken GreaterEqual() => new(SyntaxKind.GreaterEqual, ">=");

        public static SyntaxToken Lesser() => new(SyntaxKind.Lesser, "<");

        public static SyntaxToken DoubleLesser() => new(SyntaxKind.DoubleLesser, "<<");

        public static SyntaxToken DoubleLesserEqual() => new(SyntaxKind.DoubleLesserEqual, "<<=");

        public static SyntaxToken LesserEqual() => new(SyntaxKind.LesserEqual, "<=");

        public static SyntaxToken OpenParentheses() => new(SyntaxKind.OpenParenthesis, "(");

        public static SyntaxToken CloseParentheses() => new(SyntaxKind.CloseParenthesis, ")");

        public static SyntaxToken OpenBrace() => new(SyntaxKind.OpenBrace, "{");

        public static SyntaxToken CloseBrace() => new(SyntaxKind.CloseBrace, "}");

        public static SyntaxToken OpenBracket() => new(SyntaxKind.OpenBracket, "[");

        public static SyntaxToken CloseBracket() => new(SyntaxKind.CloseBracket, "]");

        public static SyntaxToken Plus() => new(SyntaxKind.Plus, "+");

        public static SyntaxToken PlusEqual() => new(SyntaxKind.PlusEqual, "+=");

        public static SyntaxToken DoublePlus() => new(SyntaxKind.DoublePlus, "++");

        public static SyntaxToken Minus() => new(SyntaxKind.Minus, "-");

        public static SyntaxToken MinusEqual() => new(SyntaxKind.MinusEqual, "-=");

        public static SyntaxToken Comma() => new(SyntaxKind.Comma, ",");

        public static SyntaxToken Percent() => new(SyntaxKind.Percent, "%");

        public static SyntaxToken PercentEqual() => new(SyntaxKind.PercentEqual, "%=");

        public static SyntaxToken DoubleAmpersand() => new(SyntaxKind.DoubleAmpersand, "&&");

        public static SyntaxToken AmpersandEqual() => new(SyntaxKind.AmpersandEqual, "&=");

        public static SyntaxToken Colon() => new(SyntaxKind.Colon, ":");

        public static SyntaxToken Caret() => new(SyntaxKind.Caret, "^");

        public static SyntaxToken CaretEqual() => new(SyntaxKind.CaretEqual, "^=");

        public static SyntaxToken Bar() => new(SyntaxKind.Bar, "|");

        public static SyntaxToken DoubleBar() => new(SyntaxKind.DoubleBar, "||");

        public static SyntaxToken BarEqual() => new(SyntaxKind.BarEqual, "|=");

        public static SyntaxToken Dot() => new(SyntaxKind.Dot, ".");

        public static SyntaxToken EndOfFile(string ending = "\n") => new(SyntaxKind.EndOfFile, string.Empty);

        public static SyntaxToken SingleLineComment(string text, string ending = "\n") => new SyntaxToken(SyntaxKind.SingleLineComment, $"//{text}").WithTrails(EndOfLine(ending));

        public static SyntaxToken TextLiteral(string text) => new(SyntaxKind.TextLiteral, $"\"{text}\"");

        public static SyntaxToken NumericalLiteral(int number) => new(SyntaxKind.NumericalLiteral, number.ToString());

        public static SyntaxToken NumericalLiteral(double number) => new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture));

        public static SyntaxToken TryKeyword() => new(SyntaxKind.TryKeyword, "try");

        public static SyntaxToken SetKeyword() => new(SyntaxKind.SetKeyword, "set");

        public static SyntaxToken DefineDirective() => new(SyntaxKind.DefineDirective, "#define");

        public static SyntaxToken IncludeDirective() => new(SyntaxKind.IncludeDirective, "#include");

        public static SyntaxToken BackwardSlashEqual() => new(SyntaxKind.BackSlashEqual, "\\=");

        public static SyntaxToken DoubleMinus() => new(SyntaxKind.DoubleMinus, "--");

        public static SyntaxToken Ampersand() => new(SyntaxKind.Ampersand, "&");

        public static SyntaxToken Question() => new(SyntaxKind.Question, "?");
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
