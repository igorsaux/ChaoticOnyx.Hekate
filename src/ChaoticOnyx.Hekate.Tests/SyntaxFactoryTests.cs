#region

using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Tests
{
    public class SyntaxFactoryTests
    {
        [Fact]
        public void SingleLineCommentTest()
        {
            // Arrange
            string      expected = "// This is a comment\n";
            SyntaxToken token    = SyntaxFactory.SingleLineComment(" This is a comment");

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
            SyntaxToken token    = SyntaxFactory.MultiLineComment("\n  Hello!\n");

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
            SyntaxToken token    = SyntaxFactory.Identifier("var");

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
            SyntaxToken token    = SyntaxFactory.TextLiteral("Test");

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

            List<SyntaxToken> tokens = new()
            {
                SyntaxFactory.NumericalLiteral(12)
                             .WithTrails(SyntaxFactory.WhiteSpace(" ")),
                SyntaxFactory.NumericalLiteral(7.8)
                             .WithTrails(SyntaxFactory.WhiteSpace(" ")),
                SyntaxFactory.NumericalLiteral((float)12.9)
            };

            // Act
            CompilationUnit unit = CompilationUnit.FromTokens(tokens.ToImmutableList());
            unit.Parse();
            string result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PathLiteralTest()
        {
            // Arrange
            string      expected = "'sound/mysound.ogg'";
            SyntaxToken token    = SyntaxFactory.PathLiteral("sound/mysound.ogg");

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
            SyntaxToken token    = SyntaxFactory.ForKeyword();

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
            SyntaxToken token    = SyntaxFactory.NewKeyword();

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
            SyntaxToken token    = SyntaxFactory.GlobalKeyword();

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
            SyntaxToken token    = SyntaxFactory.ThrowKeyword();

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
            SyntaxToken token    = SyntaxFactory.CatchKeyword();

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
            SyntaxToken token    = SyntaxFactory.TryKeyword();

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
            SyntaxToken token    = SyntaxFactory.VarKeyword();

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
            SyntaxToken token    = SyntaxFactory.VerbKeyword();

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
            SyntaxToken token    = SyntaxFactory.ProcKeyword();

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
            SyntaxToken token    = SyntaxFactory.InKeyword();

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
            SyntaxToken token    = SyntaxFactory.IfKeyword();

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
            SyntaxToken token    = SyntaxFactory.ElseKeyword();

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
            SyntaxToken token    = SyntaxFactory.SetKeyword();

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
            SyntaxToken token    = SyntaxFactory.AsKeyword();

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
            SyntaxToken token    = SyntaxFactory.WhileKeyword();

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
            SyntaxToken token    = SyntaxFactory.DefineDirective();

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
            SyntaxToken token    = SyntaxFactory.IncludeDirective();

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
            SyntaxToken token    = SyntaxFactory.IfDefDirective();

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
            SyntaxToken token    = SyntaxFactory.IfNDefDirective();

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
            SyntaxToken token    = SyntaxFactory.EndIfDirective();

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
            SyntaxToken token    = SyntaxFactory.Slash();

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
            SyntaxToken token    = SyntaxFactory.BackwardSlashEqual();

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
            SyntaxToken token    = SyntaxFactory.SlashEqual();

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
            SyntaxToken token    = SyntaxFactory.Asterisk();

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
            SyntaxToken token    = SyntaxFactory.AsteriskEqual();

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
            SyntaxToken token    = SyntaxFactory.DoubleAsterisk();

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
            SyntaxToken token    = SyntaxFactory.Equal();

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
            SyntaxToken token    = SyntaxFactory.DoubleEqual();

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
            SyntaxToken token    = SyntaxFactory.ExclamationEqual();

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
            SyntaxToken token    = SyntaxFactory.Exclamation();

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
            SyntaxToken token    = SyntaxFactory.Greater();

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
            SyntaxToken token    = SyntaxFactory.DoubleGreater();

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
            SyntaxToken token    = SyntaxFactory.DoubleGreaterEqual();

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
            SyntaxToken token    = SyntaxFactory.GreaterEqual();

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
            SyntaxToken token    = SyntaxFactory.Lesser();

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
            SyntaxToken token    = SyntaxFactory.DoubleLesser();

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
            SyntaxToken token    = SyntaxFactory.DoubleLesserEqual();

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
            SyntaxToken token    = SyntaxFactory.LesserEqual();

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
            SyntaxToken token    = SyntaxFactory.OpenParentheses();

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
            SyntaxToken token    = SyntaxFactory.CloseParentheses();

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
            SyntaxToken token    = SyntaxFactory.OpenBrace();

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
            SyntaxToken token    = SyntaxFactory.CloseBrace();

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
            SyntaxToken token    = SyntaxFactory.OpenBracket();

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
            SyntaxToken token    = SyntaxFactory.CloseBracket();

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
            SyntaxToken token    = SyntaxFactory.Plus();

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
            SyntaxToken token    = SyntaxFactory.PlusEqual();

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
            SyntaxToken token    = SyntaxFactory.DoublePlus();

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
            SyntaxToken token    = SyntaxFactory.Minus();

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
            SyntaxToken token    = SyntaxFactory.MinusEqual();

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
            SyntaxToken token    = SyntaxFactory.DoubleMinus();

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
            SyntaxToken token    = SyntaxFactory.Comma();

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
            SyntaxToken token    = SyntaxFactory.Percent();

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
            SyntaxToken token    = SyntaxFactory.PercentEqual();

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
            SyntaxToken token    = SyntaxFactory.Ampersand();

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
            SyntaxToken token    = SyntaxFactory.DoubleAmpersand();

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
            SyntaxToken token    = SyntaxFactory.AmpersandEqual();

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
            SyntaxToken token    = SyntaxFactory.Colon();

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
            SyntaxToken token    = SyntaxFactory.Question();

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
            SyntaxToken token    = SyntaxFactory.Caret();

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
            SyntaxToken token    = SyntaxFactory.CaretEqual();

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
            SyntaxToken token    = SyntaxFactory.Bar();

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
            SyntaxToken token    = SyntaxFactory.DoubleBar();

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
            SyntaxToken token    = SyntaxFactory.BarEqual();

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
            SyntaxToken token    = SyntaxFactory.Dot();

            // Act
            CompilationUnit unit   = CompilationUnit.FromToken(token);
            string          result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
