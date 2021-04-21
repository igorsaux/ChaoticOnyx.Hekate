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
            string      expected = "// This is a comment\n";
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
            string      expected = "/*\n  Hello!\n*/";
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
            string      expected = "var";
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
            string      expected = "\"Test\"";
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
            string expected = "12 7.8 12.9";

            SyntaxToken[] tokens =
            {
                _factory.NumericalLiteral(12)
                        .WithTrails(_factory.WhiteSpace(" ")),
                _factory.NumericalLiteral(7.8)
                        .WithTrails(_factory.WhiteSpace(" ")),
                _factory.NumericalLiteral((float)12.9)
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
            string      expected = "'sound/mysound.ogg'";
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
            string      expected = "for";
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
            string      expected = "new";
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
            string      expected = "global";
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
            string      expected = "throw";
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
            string      expected = "catch";
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
            string      expected = "try";
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
            string      expected = "var";
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
            string      expected = "verb";
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
            string      expected = "proc";
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
            string      expected = "in";
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
            string      expected = "if";
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
            string      expected = "else";
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
            string      expected = "set";
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
            string      expected = "as";
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
            string      expected = "while";
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
            string      expected = "#define";
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
            string      expected = "#include";
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
            string      expected = "#ifdef";
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
            string      expected = "#ifndef";
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
            string      expected = "#endif";
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
            string      expected = "/";
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
            string      expected = "\\=";
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
            string      expected = "/=";
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
            string      expected = "*";
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
            string      expected = "*=";
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
            string      expected = "**";
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
            string      expected = "=";
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
            string      expected = "==";
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
            string      expected = "!=";
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
            string      expected = "!";
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
            string      expected = ">";
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
            string      expected = ">>";
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
            string      expected = ">>=";
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
            string      expected = ">=";
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
            string      expected = "<";
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
            string      expected = "<<";
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
            string      expected = "<<=";
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
            string      expected = "<=";
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
            string      expected = "(";
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
            string      expected = ")";
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
            string      expected = "{";
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
            string      expected = "}";
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
            string      expected = "[";
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
            string      expected = "]";
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
            string      expected = "+";
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
            string      expected = "+=";
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
            string      expected = "++";
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
            string      expected = "-";
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
            string      expected = "-=";
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
            string      expected = "--";
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
            string      expected = ",";
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
            string      expected = "%";
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
            string      expected = "%=";
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
            string      expected = "&";
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
            string      expected = "&&";
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
            string      expected = "&=";
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
            string      expected = ":";
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
            string      expected = "?";
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
            string      expected = "^";
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
            string      expected = "^=";
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
            string      expected = "|";
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
            string      expected = "||";
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
            string      expected = "|=";
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
            string      expected = ".";
            SyntaxToken token    = _factory.Dot();

            // Act
            CompilationUnit unit   = CompilationUnit.FromToken(token);
            string          result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
