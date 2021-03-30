#region

using System;
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
            var expected = "// This is a comment\n";
            var token    = _factory.SingleLineComment(" This is a comment");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MultiLineCommentTest()
        {
            // Arrange
            var expected = "/*\n  Hello!\n*/";
            var token    = _factory.MultiLineComment("\n  Hello!\n");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IdentifierTest()
        {
            // Arrange
            var expected = "var";
            var token    = _factory.Identifier("var");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TextLiteralTest()
        {
            // Arrange
            var expected = "\"Test\"";
            var token    = _factory.TextLiteral("Test");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

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
            var unit   = new CompilationUnit(tokens);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PathLiteralTest()
        {
            // Arrange
            var expected = "'sound/mysound.ogg'";
            var token    = _factory.PathLiteral("sound/mysound.ogg");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ForKeywordTest()
        {
            // Arrange
            var expected = "for";
            var token    = _factory.ForKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NewKeywordTest()
        {
            // Arrange
            var expected = "new";
            var token    = _factory.NewKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GlobalKeywordTest()
        {
            // Arrange
            var expected = "global";
            var token    = _factory.GlobalKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowKeywordTest()
        {
            // Arrange
            var expected = "throw";
            var token    = _factory.ThrowKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CatchKeywordTest()
        {
            // Arrange
            var expected = "catch";
            var token    = _factory.CatchKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TryKeywordTest()
        {
            // Arrange
            var expected = "try";
            var token    = _factory.TryKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VarKeywordTest()
        {
            // Arrange
            var expected = "var";
            var token    = _factory.VarKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerbKeywordTest()
        {
            // Arrange
            var expected = "verb";
            var token    = _factory.VerbKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ProcKeywordTest()
        {
            // Arrange
            var expected = "proc";
            var token    = _factory.ProcKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InKeywordTest()
        {
            // Arrange
            var expected = "in";
            var token    = _factory.InKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfKeywordTest()
        {
            // Arrange
            var expected = "if";
            var token    = _factory.IfKeyword();

            // Act
            var unit  = new CompilationUnit(token);
            var resul = unit.Emit();

            // Assert
            Assert.Equal(expected, resul);
        }

        [Fact]
        public void ElseKeywordTest()
        {
            // Arrange
            var expected = "else";
            var token    = _factory.ElseKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetKeywordTest()
        {
            // Arrange
            var expected = "set";
            var token    = _factory.SetKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsKeywordTest()
        {
            // Arrange
            var expected = "as";
            var token    = _factory.AsKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhileKeywordTest()
        {
            // Arrange
            var expected = "while";
            var token    = _factory.WhileKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DefineDirectiveTest()
        {
            // Arrange
            var expected = "#define";
            var token    = _factory.DefineDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IncludeDirectiveTest()
        {
            // Arrange
            var expected = "#include";
            var token    = _factory.IncludeDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfDefDirectiveTest()
        {
            // Arrange
            var expected = "#ifdef";
            var token    = _factory.IfDefDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfNDefDirectiveTest()
        {
            // Arrange
            var expected = "#ifndef";
            var token    = _factory.IfNDefDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EndIfDirectiveTest()
        {
            // Arrange
            var expected = "#endif";
            var token    = _factory.EndIfDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SlashTest()
        {
            // Arrange
            var expected = "/";
            var token    = _factory.Slash();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BackwardSlashEqualTest()
        {
            // Arrange
            var expected = "\\=";
            var token    = _factory.BackwardSlashEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SlashEqualTest()
        {
            // Arrange
            var expected = "/=";
            var token    = _factory.SlashEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsteriskTest()
        {
            // Arrange
            var expected = "*";
            var token    = _factory.Asterisk();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsteriskEqualTest()
        {
            // Arrange
            var expected = "*=";
            var token    = _factory.AsteriskEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleAsteriskTest()
        {
            // Arrange
            var expected = "**";
            var token    = _factory.DoubleAsterisk();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualTest()
        {
            // Arrange
            var expected = "=";
            var token    = _factory.Equal();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleEqualTest()
        {
            // Arrange
            var expected = "==";
            var token    = _factory.DoubleEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExclamationEqualTest()
        {
            // Arrange
            var expected = "!=";
            var token    = _factory.ExclamationEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExclamantionTest()
        {
            // Arrange
            var expected = "!";
            var token    = _factory.Exclamation();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GreaterTest()
        {
            // Arrange
            var expected = ">";
            var token    = _factory.Greater();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleGreaterTest()
        {
            // Arrange
            var expected = ">>";
            var token    = _factory.DoubleGreater();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleGreaterEqualTest()
        {
            // Arrange
            var expected = ">>=";
            var token    = _factory.DoubleGreaterEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GreaterEqualTest()
        {
            // Arrange
            var expected = ">=";
            var token    = _factory.GreaterEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void LesserTest()
        {
            // Arrange
            var expected = "<";
            var token    = _factory.Lesser();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleLesserTest()
        {
            // Arrange
            var expected = "<<";
            var token    = _factory.DoubleLesser();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleLesserEqualTest()
        {
            // Arrange
            var expected = "<<=";
            var token    = _factory.DoubleLesserEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void LesserEqualTest()
        {
            // Arrange
            var expected = "<=";
            var token    = _factory.LesserEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OpenParenthesesTest()
        {
            // Arrange
            var expected = "(";
            var token    = _factory.OpenParentheses();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CloseParenthesesTest()
        {
            // Arrange
            var expected = ")";
            var token    = _factory.CloseParentheses();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OpenBraceTest()
        {
            // Arrange
            var expected = "{";
            var token    = _factory.OpenBrace();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CloseBraceTest()
        {
            // Arrange
            var expected = "}";
            var token    = _factory.CloseBrace();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OpenBracketTest()
        {
            // Arrange
            var expected = "[";
            var token    = _factory.OpenBracket();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CloseBracketTest()
        {
            // Arrange
            var expected = "]";
            var token    = _factory.CloseBracket();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PlusTest()
        {
            // Arrange
            var expected = "+";
            var token    = _factory.Plus();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PlusEqualTest()
        {
            // Arrange
            var expected = "+=";
            var token    = _factory.PlusEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoublePlusTest()
        {
            // Arrange
            var expected = "++";
            var token    = _factory.DoublePlus();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MinusTest()
        {
            // Arrange
            var expected = "-";
            var token    = _factory.Minus();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MinusEqualTest()
        {
            // Arrange
            var expected = "-=";
            var token    = _factory.MinusEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleMinusTest()
        {
            // Arrange
            var expected = "--";
            var token    = _factory.DoubleMinus();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CommaTest()
        {
            // Arrange
            var expected = ",";
            var token    = _factory.Comma();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PercentTest()
        {
            // Arrange
            var expected = "%";
            var token    = _factory.Percent();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PercentEqualTest()
        {
            // Arrange
            var expected = "%=";
            var token    = _factory.PercentEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AmpersandTest()
        {
            // Arrange
            var expected = "&";
            var token    = _factory.Ampersand();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleAmpersandTest()
        {
            // Arrange
            var expected = "&&";
            var token    = _factory.DoubleAmpersand();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AmpersandEqualTest()
        {
            // Arrange
            var expected = "&=";
            var token    = _factory.AmpersandEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ColonTest()
        {
            // Arrange
            var expected = ":";
            var token    = _factory.Colon();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void QuestionTest()
        {
            // Arrange
            var expected = "?";
            var token    = _factory.Question();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CaretTest()
        {
            // Arrange
            var expected = "^";
            var token    = _factory.Caret();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CaretEqualTest()
        {
            // Arrange
            var expected = "^=";
            var token    = _factory.CaretEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BarTest()
        {
            // Arrange
            var expected = "|";
            var token    = _factory.Bar();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleBarTest()
        {
            // Arrange
            var expected = "||";
            var token    = _factory.DoubleBar();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BarEqualTest()
        {
            // Arrange
            var expected = "|=";
            var token    = _factory.BarEqual();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DotTest()
        {
            // Arrange
            var expected = ".";
            var token    = _factory.Dot();
            
            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();
            
            // Assert
            Assert.Equal(expected, result);
        }
    }
}
