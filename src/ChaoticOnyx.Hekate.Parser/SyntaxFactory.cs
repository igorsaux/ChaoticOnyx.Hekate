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

		private SyntaxFactory(CodeStyle style) { Style = style; }

		public static SyntaxFactory CreateFactory(CodeStyle style) { return new(style); }

		public SyntaxToken WhiteSpace(string space) { return new(SyntaxKind.WhiteSpace, space); }

		public SyntaxToken EndOfLine(string ending = "\n") { return new(SyntaxKind.EndOfLine, ending); }

		public SyntaxToken EndOfFile(string ending = "\n")
		{
			SyntaxToken token = new(SyntaxKind.EndOfFile, string.Empty);

			if (Style.LastEmptyLine) { token.WithLeads(EndOfLine(ending)); }

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

		public SyntaxToken Identifier(string name) { return new(SyntaxKind.Identifier, name); }

		public SyntaxToken TextLiteral(string text) { return new(SyntaxKind.TextLiteral, $"\"{text}\""); }

		public SyntaxToken NumericalLiteral(int number) { return new(SyntaxKind.NumericalLiteral, number.ToString()); }

		public SyntaxToken NumericalLiteral(float number) { return new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture)); }

		public SyntaxToken NumericalLiteral(double number) { return new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture)); }

		public SyntaxToken PathLiteral(string path) { return new(SyntaxKind.PathLiteral, $"'{path}'"); }

		public SyntaxToken ForKeyword() { return new(SyntaxKind.ForKeyword, "for"); }

		public SyntaxToken NewKeyword() { return new(SyntaxKind.NewKeyword, "new"); }

		public SyntaxToken GlobalKeyword() { return new(SyntaxKind.GlobalKeyword, "global"); }

		public SyntaxToken ThrowKeyword() { return new(SyntaxKind.ThrowKeyword, "throw"); }

		public SyntaxToken CatchKeyword() { return new(SyntaxKind.CatchKeyword, "catch"); }

		public SyntaxToken TryKeyword() { return new(SyntaxKind.TryKeyword, "try"); }

		public SyntaxToken VarKeyword() { return new(SyntaxKind.VarKeyword, "var"); }

		public SyntaxToken VerbKeyword() { return new(SyntaxKind.VerbKeyword, "verb"); }

		public SyntaxToken ProcKeyword() { return new(SyntaxKind.ProcKeyword, "proc"); }

		public SyntaxToken InKeyword() { return new(SyntaxKind.InKeyword, "in"); }

		public SyntaxToken IfKeyword() { return new(SyntaxKind.IfKeyword, "if"); }

		public SyntaxToken ElseKeyword() { return new(SyntaxKind.ElseKeyword, "else"); }

		public SyntaxToken SetKeyword() { return new(SyntaxKind.SetKeyword, "set"); }

		public SyntaxToken AsKeyword() { return new(SyntaxKind.AsKeyword, "as"); }

		public SyntaxToken WhileKeyword() { return new(SyntaxKind.WhileKeyword, "while"); }

		public SyntaxToken DefineDirective() { return new(SyntaxKind.DefineDirective, "#define"); }

		public SyntaxToken IncludeDirective() { return new(SyntaxKind.IncludeDirective, "#include"); }

		public SyntaxToken IfDefDirective() { return new(SyntaxKind.IfDefDirective, "#ifdef"); }

		public SyntaxToken IfNDefDirective() { return new(SyntaxKind.IfNDefDirective, "#ifndef"); }

		public SyntaxToken EndIfDirective() { return new(SyntaxKind.EndIfDirective, "#endif"); }

		public SyntaxToken Slash() { return new(SyntaxKind.Slash, "/"); }

		public SyntaxToken BackwardSlashEqual() { return new(SyntaxKind.BackwardSlashEqual, "\\="); }

		public SyntaxToken SlashEqual() { return new(SyntaxKind.SlashEqual, "/="); }

		public SyntaxToken Asterisk() { return new(SyntaxKind.Asterisk, "*"); }

		public SyntaxToken AsteriskEqual() { return new(SyntaxKind.AsteriskEqual, "*="); }

		public SyntaxToken DoubleAsterisk() { return new(SyntaxKind.DoubleAsterisk, "**"); }

		public SyntaxToken Equal() { return new(SyntaxKind.Equal, "="); }

		public SyntaxToken DoubleEqual() { return new(SyntaxKind.DoubleEqual, "=="); }

		public SyntaxToken ExclamationEqual() { return new(SyntaxKind.ExclamationEqual, "!="); }

		public SyntaxToken Exclamation() { return new(SyntaxKind.Exclamation, "!"); }

		public SyntaxToken Greater() { return new(SyntaxKind.Greater, ">"); }

		public SyntaxToken DoubleGreater() { return new(SyntaxKind.DoubleGreater, ">>"); }

		public SyntaxToken DoubleGreaterEqual() { return new(SyntaxKind.DoubleGreaterEqual, ">>="); }

		public SyntaxToken GreaterEqual() { return new(SyntaxKind.GreaterEqual, ">="); }

		public SyntaxToken Lesser() { return new(SyntaxKind.Lesser, "<"); }

		public SyntaxToken DoubleLesser() { return new(SyntaxKind.DoubleLesser, "<<"); }

		public SyntaxToken DoubleLesserEqual() { return new(SyntaxKind.DoubleLesserEqual, "<<="); }

		public SyntaxToken LesserEqual() { return new(SyntaxKind.LesserEqual, "<="); }

		public SyntaxToken OpenParentheses() { return new(SyntaxKind.OpenParenthesis, "("); }

		public SyntaxToken CloseParentheses() { return new(SyntaxKind.CloseParenthesis, ")"); }

		public SyntaxToken OpenBrace() { return new(SyntaxKind.OpenBrace, "{"); }

		public SyntaxToken CloseBrace() { return new(SyntaxKind.CloseBrace, "}"); }

		public SyntaxToken OpenBracket() { return new(SyntaxKind.OpenBracket, "["); }

		public SyntaxToken CloseBracket() { return new(SyntaxKind.CloseBracket, "]"); }

		public SyntaxToken Plus() { return new(SyntaxKind.Plus, "+"); }

		public SyntaxToken PlusEqual() { return new(SyntaxKind.PlusEqual, "+="); }

		public SyntaxToken DoublePlus() { return new(SyntaxKind.DoublePlus, "++"); }

		public SyntaxToken Minus() { return new(SyntaxKind.Minus, "-"); }

		public SyntaxToken MinusEqual() { return new(SyntaxKind.MinusEqual, "-="); }

		public SyntaxToken DoubleMinus() { return new(SyntaxKind.DoubleMinus, "--"); }

		public SyntaxToken Comma() { return new(SyntaxKind.Comma, ","); }

		public SyntaxToken Percent() { return new(SyntaxKind.Percent, "%"); }

		public SyntaxToken PercentEqual() { return new(SyntaxKind.PercentEqual, "%="); }

		public SyntaxToken Ampersand() { return new(SyntaxKind.Ampersand, "&"); }

		public SyntaxToken DoubleAmpersand() { return new(SyntaxKind.DoubleAmpersand, "&&"); }

		public SyntaxToken AmpersandEqual() { return new(SyntaxKind.AmpersandEqual, "&="); }

		public SyntaxToken Colon() { return new(SyntaxKind.Colon, ":"); }

		public SyntaxToken Question() { return new(SyntaxKind.Question, "?"); }

		public SyntaxToken Caret() { return new(SyntaxKind.Caret, "^"); }

		public SyntaxToken CaretEqual() { return new(SyntaxKind.CaretEqual, "^="); }

		public SyntaxToken Bar() { return new(SyntaxKind.Bar, "|"); }

		public SyntaxToken DoubleBar() { return new(SyntaxKind.DoubleBar, "||"); }

		public SyntaxToken BarEqual() { return new(SyntaxKind.BarEqual, "|="); }

		public SyntaxToken Dot() { return new(SyntaxKind.Dot, "."); }

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
