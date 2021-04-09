#region

using System.Collections.Generic;
using System.Linq;
using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Parser.Tests
{
	public class LexerParsingTests
	{
		[Fact]
		public void CommentParsing()
		{
			// Arrange
			// Act
			CompilationUnit unit = CompilationUnit.FromSource(@"/* MultiLine Comment*/

// SingleLine Comment", modes: ParsingModes.None);

			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 1);

			Assert.True(tokens[0]
							.Kind == SyntaxKind.EndOfFile);

			Assert.True(tokens[0]
						.Leads.Count == 4);

			Assert.True(tokens[0]
						.Leads[0]
						.Kind == SyntaxKind.MultiLineComment);

			Assert.True(tokens[0]
						.Leads.Count(t => t.Kind == SyntaxKind.EndOfLine) == 2);

			Assert.True(tokens[0]
						.Leads[3]
						.Kind == SyntaxKind.SingleLineComment);
		}

		[Fact]
		public void IdentifierParsing()
		{
			// Arrange
			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource("literal", modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 2);

			Assert.True(tokens[0]
							.Kind == SyntaxKind.Identifier);

			Assert.True(tokens[0]
							.Text == "literal");
		}

		[Fact]
		public void NumericalLiteralParsing()
		{
			// Arrange
			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource("123", modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 2);

			Assert.True(tokens[0]
							.Kind == SyntaxKind.NumericalLiteral);

			Assert.True(tokens[0]
							.Text == "123");
		}

		[Fact]
		public void FloatNumericalLiteralParsing()
		{
			// Arrange
			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource("123.55", modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 2);

			Assert.True(tokens[0]
							.Kind == SyntaxKind.NumericalLiteral);

			Assert.True(tokens[0]
							.Text == "123.55");
		}

		[Fact]
		public void TextLiteralParsing()
		{
			// Arrange
			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource("\"TextLiteral\"", modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 2);

			Assert.True(tokens[0]
							.Kind == SyntaxKind.TextLiteral);
		}

		[Fact]
		public void PathLiteralParsing()
		{
			// Arrange
			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource("\'PathLiteral/file.dm\'", modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 2);

			Assert.True(tokens[0]
							.Kind == SyntaxKind.PathLiteral);
		}

		[Fact]
		public void SpacesParsing()
		{
			// Arrange
			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource(@"    // Comment", modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 1);

			Assert.True(tokens[0]
						.Leads.Count == 2);

			Assert.True(tokens[0]
						.Leads[0]
						.Kind == SyntaxKind.WhiteSpace);

			Assert.True(tokens[0]
						.Leads[1]
						.Kind == SyntaxKind.SingleLineComment);
		}

		[Theory]
		[InlineData(SyntaxKind.IncludeDirective)]
		[InlineData(SyntaxKind.IfNDefDirective)]
		[InlineData(SyntaxKind.IfDefDirective)]
		[InlineData(SyntaxKind.EndIfDirective)]
		[InlineData(SyntaxKind.DefineDirective)]
		[InlineData(SyntaxKind.UndefDirective)]
		public void DirectiveParsing(SyntaxKind kind)
		{
			// Arrange
			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource("#include #ifndef #ifdef #endif #define #undef", modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 7);
			Assert.True(tokens.Count(t => t.Kind == kind) == 1);
		}

		[Theory]
		[InlineData(SyntaxKind.ForKeyword)]
		[InlineData(SyntaxKind.NewKeyword)]
		[InlineData(SyntaxKind.GlobalKeyword)]
		[InlineData(SyntaxKind.ThrowKeyword)]
		[InlineData(SyntaxKind.CatchKeyword)]
		[InlineData(SyntaxKind.TryKeyword)]
		[InlineData(SyntaxKind.VarKeyword)]
		[InlineData(SyntaxKind.VerbKeyword)]
		[InlineData(SyntaxKind.ProcKeyword)]
		[InlineData(SyntaxKind.InKeyword)]
		[InlineData(SyntaxKind.IfKeyword)]
		[InlineData(SyntaxKind.ElseKeyword)]
		[InlineData(SyntaxKind.SetKeyword)]
		[InlineData(SyntaxKind.AsKeyword)]
		[InlineData(SyntaxKind.WhileKeyword)]
		public void KeywordParsing(SyntaxKind kind)
		{
			// Arrange
			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource("for new global throw catch try var verb proc in if else set as while", modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 16);
			Assert.True(tokens.Count(t => t.Kind == kind) == 1);
		}

		[Theory]
		[InlineData(SyntaxKind.Asterisk)]
		[InlineData(SyntaxKind.AsteriskEqual)]
		[InlineData(SyntaxKind.Equal, 2)]
		[InlineData(SyntaxKind.DoubleEqual, 1)]
		[InlineData(SyntaxKind.ExclamationEqual, 1)]
		[InlineData(SyntaxKind.Exclamation, 1)]
		[InlineData(SyntaxKind.Greater)]
		[InlineData(SyntaxKind.DoubleGreater)]
		[InlineData(SyntaxKind.DoubleGreaterEqual)]
		[InlineData(SyntaxKind.GreaterEqual)]
		[InlineData(SyntaxKind.Lesser)]
		[InlineData(SyntaxKind.DoubleLesser)]
		[InlineData(SyntaxKind.DoubleLesserEqual)]
		[InlineData(SyntaxKind.LesserEqual)]
		[InlineData(SyntaxKind.OpenParenthesis)]
		[InlineData(SyntaxKind.CloseParenthesis)]
		[InlineData(SyntaxKind.OpenBrace)]
		[InlineData(SyntaxKind.CloseBrace)]
		[InlineData(SyntaxKind.Plus)]
		[InlineData(SyntaxKind.DoublePlus)]
		[InlineData(SyntaxKind.PlusEqual)]
		[InlineData(SyntaxKind.Minus)]
		[InlineData(SyntaxKind.DoubleMinus)]
		[InlineData(SyntaxKind.MinusEqual)]
		[InlineData(SyntaxKind.Comma, 2)]
		[InlineData(SyntaxKind.DoubleAsterisk)]
		[InlineData(SyntaxKind.Ampersand)]
		[InlineData(SyntaxKind.AmpersandEqual)]
		[InlineData(SyntaxKind.DoubleAmpersand)]
		[InlineData(SyntaxKind.Percent)]
		[InlineData(SyntaxKind.PercentEqual)]
		[InlineData(SyntaxKind.Colon)]
		[InlineData(SyntaxKind.Question)]
		[InlineData(SyntaxKind.Caret)]
		[InlineData(SyntaxKind.CaretEqual)]
		[InlineData(SyntaxKind.Bar)]
		[InlineData(SyntaxKind.DoubleBar)]
		[InlineData(SyntaxKind.BarEqual)]
		[InlineData(SyntaxKind.Unknown, 1)]
		[InlineData(SyntaxKind.PathLiteral)]
		[InlineData(SyntaxKind.TextLiteral)]
		[InlineData(SyntaxKind.SlashEqual)]
		[InlineData(SyntaxKind.Slash)]
		public void CheckTokenParsing(SyntaxKind kind, int expectedCount = 1)
		{
			// Arrange
			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource("* *= \\= '' \"\" / == = =!!= >= > >> >>= <= < << <<= () {} [] + ++ += - -- -=,, ** & &=&& /= % %= : ? ^ ^= | |= || \\ .", modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;
			int                count  = tokens.Count(token => token.Kind == kind);

			// Assert
			Assert.Equal(expectedCount, count);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(8)]
		public void TabSizeTest(int tabSize)
		{
			// Arrange
			var text = "\t\tvar";
			int tabs = text.Count(c => c == '\t');

			// Act
			CompilationUnit unit  = CompilationUnit.FromSource(text, tabSize, ParsingModes.None);
			SyntaxToken     token = unit.Lexer.Tokens[0];

			// Assert
			Assert.True(token.FilePosition.Column == 1 + tabs * tabSize);
		}

		[Fact]
		public void EscapedTextTest()
		{
			// Arrange
			var text = @"var/a = ""chemical_reactions_list\[\""[reaction]\""\] = \""[chemical_reactions_list[reaction]]\""\n""";

			// Act
			CompilationUnit    unit   = CompilationUnit.FromSource(text, modes: ParsingModes.None);
			IList<SyntaxToken> tokens = unit.Lexer.Tokens;

			// Assert
			Assert.True(tokens.Count == 6);

			Assert.True(tokens[0]
							.Kind == SyntaxKind.VarKeyword);

			Assert.True(tokens[1]
							.Kind == SyntaxKind.Slash);

			Assert.True(tokens[2]
							.Kind == SyntaxKind.Identifier);

			Assert.True(tokens[3]
							.Kind == SyntaxKind.Equal);

			Assert.True(tokens[4]
							.Kind == SyntaxKind.TextLiteral);

			Assert.True(tokens[5]
							.Kind == SyntaxKind.EndOfFile);
		}
	}
}
