#region

using ChaoticOnyx.Hekate.Parser.ChaoticOnyx.Tools.StyleCop;
using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Parser.Tests
{
	public class SyntaxFactoryTests
	{
		private readonly SyntaxFactory _factory = SyntaxFactory.CreateFactory(CodeStyle.Default);

		[Fact]
		public void SingleLineCommentTest()
		{
			// Arrange
			var         expected = "// This is a comment\n";
			SyntaxToken token    = _factory.SingleLineComment(" This is a comment");

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void MultiLineCommentTest()
		{
			// Arrange
			var         expected = "/*\n  Hello!\n*/";
			SyntaxToken token    = _factory.MultiLineComment("\n  Hello!\n");

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void IdentifierTest()
		{
			// Arrange
			var         expected = "var";
			SyntaxToken token    = _factory.Identifier("var");

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void TextLiteralTest()
		{
			// Arrange
			var         expected = "\"Test\"";
			SyntaxToken token    = _factory.TextLiteral("Test");

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void NumericalLiteralTest()
		{
			// Arrange
			var expected = "12 7.8 12.9";

			SyntaxToken[] tokens =
			{
				_factory.NumericalLiteral(12)
						.WithTrails(_factory.WhiteSpace(" ")),
				_factory.NumericalLiteral(7.8)
						.WithTrails(_factory.WhiteSpace(" ")),
				_factory.NumericalLiteral((float) 12.9)
			};

			// Act
			CompilationUnit unit   = CompilationUnit.FromTokens(tokens);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void PathLiteralTest()
		{
			// Arrange
			var         expected = "'sound/mysound.ogg'";
			SyntaxToken token    = _factory.PathLiteral("sound/mysound.ogg");

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void ForKeywordTest()
		{
			// Arrange
			var         expected = "for";
			SyntaxToken token    = _factory.ForKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void NewKeywordTest()
		{
			// Arrange
			var         expected = "new";
			SyntaxToken token    = _factory.NewKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void GlobalKeywordTest()
		{
			// Arrange
			var         expected = "global";
			SyntaxToken token    = _factory.GlobalKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void ThrowKeywordTest()
		{
			// Arrange
			var         expected = "throw";
			SyntaxToken token    = _factory.ThrowKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void CatchKeywordTest()
		{
			// Arrange
			var         expected = "catch";
			SyntaxToken token    = _factory.CatchKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void TryKeywordTest()
		{
			// Arrange
			var         expected = "try";
			SyntaxToken token    = _factory.TryKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void VarKeywordTest()
		{
			// Arrange
			var         expected = "var";
			SyntaxToken token    = _factory.VarKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void VerbKeywordTest()
		{
			// Arrange
			var         expected = "verb";
			SyntaxToken token    = _factory.VerbKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void ProcKeywordTest()
		{
			// Arrange
			var         expected = "proc";
			SyntaxToken token    = _factory.ProcKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void InKeywordTest()
		{
			// Arrange
			var         expected = "in";
			SyntaxToken token    = _factory.InKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void IfKeywordTest()
		{
			// Arrange
			var         expected = "if";
			SyntaxToken token    = _factory.IfKeyword();

			// Act
			CompilationUnit unit  = CompilationUnit.FromToken(token);
			string          resul = unit.Emit();

			// Assert
			Assert.Equal(expected, resul);
		}

		[Fact]
		public void ElseKeywordTest()
		{
			// Arrange
			var         expected = "else";
			SyntaxToken token    = _factory.ElseKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void SetKeywordTest()
		{
			// Arrange
			var         expected = "set";
			SyntaxToken token    = _factory.SetKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void AsKeywordTest()
		{
			// Arrange
			var         expected = "as";
			SyntaxToken token    = _factory.AsKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void WhileKeywordTest()
		{
			// Arrange
			var         expected = "while";
			SyntaxToken token    = _factory.WhileKeyword();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DefineDirectiveTest()
		{
			// Arrange
			var         expected = "#define";
			SyntaxToken token    = _factory.DefineDirective();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void IncludeDirectiveTest()
		{
			// Arrange
			var         expected = "#include";
			SyntaxToken token    = _factory.IncludeDirective();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void IfDefDirectiveTest()
		{
			// Arrange
			var         expected = "#ifdef";
			SyntaxToken token    = _factory.IfDefDirective();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void IfNDefDirectiveTest()
		{
			// Arrange
			var         expected = "#ifndef";
			SyntaxToken token    = _factory.IfNDefDirective();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void EndIfDirectiveTest()
		{
			// Arrange
			var         expected = "#endif";
			SyntaxToken token    = _factory.EndIfDirective();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void SlashTest()
		{
			// Arrange
			var         expected = "/";
			SyntaxToken token    = _factory.Slash();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void BackwardSlashEqualTest()
		{
			// Arrange
			var         expected = "\\=";
			SyntaxToken token    = _factory.BackwardSlashEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void SlashEqualTest()
		{
			// Arrange
			var         expected = "/=";
			SyntaxToken token    = _factory.SlashEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void AsteriskTest()
		{
			// Arrange
			var         expected = "*";
			SyntaxToken token    = _factory.Asterisk();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void AsteriskEqualTest()
		{
			// Arrange
			var         expected = "*=";
			SyntaxToken token    = _factory.AsteriskEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoubleAsteriskTest()
		{
			// Arrange
			var         expected = "**";
			SyntaxToken token    = _factory.DoubleAsterisk();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void EqualTest()
		{
			// Arrange
			var         expected = "=";
			SyntaxToken token    = _factory.Equal();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoubleEqualTest()
		{
			// Arrange
			var         expected = "==";
			SyntaxToken token    = _factory.DoubleEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void ExclamationEqualTest()
		{
			// Arrange
			var         expected = "!=";
			SyntaxToken token    = _factory.ExclamationEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void ExclamantionTest()
		{
			// Arrange
			var         expected = "!";
			SyntaxToken token    = _factory.Exclamation();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void GreaterTest()
		{
			// Arrange
			var         expected = ">";
			SyntaxToken token    = _factory.Greater();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoubleGreaterTest()
		{
			// Arrange
			var         expected = ">>";
			SyntaxToken token    = _factory.DoubleGreater();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoubleGreaterEqualTest()
		{
			// Arrange
			var         expected = ">>=";
			SyntaxToken token    = _factory.DoubleGreaterEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void GreaterEqualTest()
		{
			// Arrange
			var         expected = ">=";
			SyntaxToken token    = _factory.GreaterEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void LesserTest()
		{
			// Arrange
			var         expected = "<";
			SyntaxToken token    = _factory.Lesser();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoubleLesserTest()
		{
			// Arrange
			var         expected = "<<";
			SyntaxToken token    = _factory.DoubleLesser();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoubleLesserEqualTest()
		{
			// Arrange
			var         expected = "<<=";
			SyntaxToken token    = _factory.DoubleLesserEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void LesserEqualTest()
		{
			// Arrange
			var         expected = "<=";
			SyntaxToken token    = _factory.LesserEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void OpenParenthesesTest()
		{
			// Arrange
			var         expected = "(";
			SyntaxToken token    = _factory.OpenParentheses();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void CloseParenthesesTest()
		{
			// Arrange
			var         expected = ")";
			SyntaxToken token    = _factory.CloseParentheses();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void OpenBraceTest()
		{
			// Arrange
			var         expected = "{";
			SyntaxToken token    = _factory.OpenBrace();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void CloseBraceTest()
		{
			// Arrange
			var         expected = "}";
			SyntaxToken token    = _factory.CloseBrace();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void OpenBracketTest()
		{
			// Arrange
			var         expected = "[";
			SyntaxToken token    = _factory.OpenBracket();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void CloseBracketTest()
		{
			// Arrange
			var         expected = "]";
			SyntaxToken token    = _factory.CloseBracket();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void PlusTest()
		{
			// Arrange
			var         expected = "+";
			SyntaxToken token    = _factory.Plus();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void PlusEqualTest()
		{
			// Arrange
			var         expected = "+=";
			SyntaxToken token    = _factory.PlusEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoublePlusTest()
		{
			// Arrange
			var         expected = "++";
			SyntaxToken token    = _factory.DoublePlus();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void MinusTest()
		{
			// Arrange
			var         expected = "-";
			SyntaxToken token    = _factory.Minus();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void MinusEqualTest()
		{
			// Arrange
			var         expected = "-=";
			SyntaxToken token    = _factory.MinusEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoubleMinusTest()
		{
			// Arrange
			var         expected = "--";
			SyntaxToken token    = _factory.DoubleMinus();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void CommaTest()
		{
			// Arrange
			var         expected = ",";
			SyntaxToken token    = _factory.Comma();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void PercentTest()
		{
			// Arrange
			var         expected = "%";
			SyntaxToken token    = _factory.Percent();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void PercentEqualTest()
		{
			// Arrange
			var         expected = "%=";
			SyntaxToken token    = _factory.PercentEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void AmpersandTest()
		{
			// Arrange
			var         expected = "&";
			SyntaxToken token    = _factory.Ampersand();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoubleAmpersandTest()
		{
			// Arrange
			var         expected = "&&";
			SyntaxToken token    = _factory.DoubleAmpersand();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void AmpersandEqualTest()
		{
			// Arrange
			var         expected = "&=";
			SyntaxToken token    = _factory.AmpersandEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void ColonTest()
		{
			// Arrange
			var         expected = ":";
			SyntaxToken token    = _factory.Colon();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void QuestionTest()
		{
			// Arrange
			var         expected = "?";
			SyntaxToken token    = _factory.Question();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void CaretTest()
		{
			// Arrange
			var         expected = "^";
			SyntaxToken token    = _factory.Caret();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void CaretEqualTest()
		{
			// Arrange
			var         expected = "^=";
			SyntaxToken token    = _factory.CaretEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void BarTest()
		{
			// Arrange
			var         expected = "|";
			SyntaxToken token    = _factory.Bar();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DoubleBarTest()
		{
			// Arrange
			var         expected = "||";
			SyntaxToken token    = _factory.DoubleBar();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void BarEqualTest()
		{
			// Arrange
			var         expected = "|=";
			SyntaxToken token    = _factory.BarEqual();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void DotTest()
		{
			// Arrange
			var         expected = ".";
			SyntaxToken token    = _factory.Dot();

			// Act
			CompilationUnit unit   = CompilationUnit.FromToken(token);
			string          result = unit.Emit();

			// Assert
			Assert.Equal(expected, result);
		}
	}
}
