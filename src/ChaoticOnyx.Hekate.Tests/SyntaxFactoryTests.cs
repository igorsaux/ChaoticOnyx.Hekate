#region

using System.Collections.Generic;
using ChaoticOnyx.Hekate.Scaffolds;
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
            string expected = "// This is a comment\n";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.SingleLineComment(" This is a comment")
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MultiLineCommentTest()
        {
            // Arrange
            string expected = "/*\n  Hello!\n*/";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.MultiLineComment("\n  Hello!\n")
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IdentifierTest()
        {
            // Arrange
            string expected = "var";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Identifier("var")
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TextLiteralTest()
        {
            // Arrange
            string expected = "\"Test\"";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.TextLiteral("Test")
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NumericalLiteralTest()
        {
            // Arrange
            string expected = "12 7.8 12.9";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.NumericalLiteral(12)
                             .WithTrails(SyntaxFactory.WhiteSpace(" ")),
                SyntaxFactory.NumericalLiteral(7.8)
                             .WithTrails(SyntaxFactory.WhiteSpace(" ")),
                SyntaxFactory.NumericalLiteral((float)12.9)
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PathLiteralTest()
        {
            // Arrange
            string expected = "'sound/mysound.ogg'";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.PathLiteral("sound/mysound.ogg")
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ForKeywordTest()
        {
            // Arrange
            string expected = "for";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.ForKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NewKeywordTest()
        {
            // Arrange
            string expected = "new";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.NewKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GlobalKeywordTest()
        {
            // Arrange
            string expected = "global";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.GlobalKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowKeywordTest()
        {
            // Arrange
            string expected = "throw";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.ThrowKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CatchKeywordTest()
        {
            // Arrange
            string expected = "catch";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.CatchKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TryKeywordTest()
        {
            // Arrange
            string expected = "try";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.TryKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VarKeywordTest()
        {
            // Arrange
            string expected = "var";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.VarKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerbKeywordTest()
        {
            // Arrange
            string expected = "verb";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.VerbKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ProcKeywordTest()
        {
            // Arrange
            string expected = "proc";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.ProcKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InKeywordTest()
        {
            // Arrange
            string expected = "in";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.InKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfKeywordTest()
        {
            // Arrange
            string expected = "if";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.IfKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ElseKeywordTest()
        {
            // Arrange
            string expected = "else";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.ElseKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetKeywordTest()
        {
            // Arrange
            string expected = "set";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.SetKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsKeywordTest()
        {
            // Arrange
            string expected = "as";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.AsKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhileKeywordTest()
        {
            // Arrange
            string expected = "while";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.WhileKeyword()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DefineDirectiveTest()
        {
            // Arrange
            string expected = "#define";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DefineDirective()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IncludeDirectiveTest()
        {
            // Arrange
            string expected = "#include";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.IncludeDirective()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfDefDirectiveTest()
        {
            // Arrange
            string expected = "#ifdef";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.IfDefDirective()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfNDefDirectiveTest()
        {
            // Arrange
            string expected = "#ifndef";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.IfNDefDirective()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EndIfDirectiveTest()
        {
            // Arrange
            string expected = "#endif";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.EndIfDirective()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SlashTest()
        {
            // Arrange
            string expected = "/";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Slash()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BackwardSlashEqualTest()
        {
            // Arrange
            string expected = "\\=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.BackwardSlashEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SlashEqualTest()
        {
            // Arrange
            string expected = "/=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.SlashEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsteriskTest()
        {
            // Arrange
            string expected = "*";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Asterisk()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsteriskEqualTest()
        {
            // Arrange
            string expected = "*=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.AsteriskEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleAsteriskTest()
        {
            // Arrange
            string expected = "**";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoubleAsterisk()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualTest()
        {
            // Arrange
            string expected = "=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Equal()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleEqualTest()
        {
            // Arrange
            string expected = "==";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoubleEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExclamationEqualTest()
        {
            // Arrange
            string expected = "!=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.ExclamationEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExclamantionTest()
        {
            // Arrange
            string expected = "!";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Exclamation()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GreaterTest()
        {
            // Arrange
            string expected = ">";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Greater()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleGreaterTest()
        {
            // Arrange
            string expected = ">>";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoubleGreater()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleGreaterEqualTest()
        {
            // Arrange
            string expected = ">>=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoubleGreaterEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GreaterEqualTest()
        {
            // Arrange
            string expected = ">=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.GreaterEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void LesserTest()
        {
            // Arrange
            string expected = "<";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Lesser()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleLesserTest()
        {
            // Arrange
            string expected = "<<";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoubleLesser()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleLesserEqualTest()
        {
            // Arrange
            string expected = "<<=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoubleLesserEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void LesserEqualTest()
        {
            // Arrange
            string expected = "<=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.LesserEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OpenParenthesesTest()
        {
            // Arrange
            string expected = "(";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.OpenParentheses()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CloseParenthesesTest()
        {
            // Arrange
            string expected = ")";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.CloseParentheses()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OpenBraceTest()
        {
            // Arrange
            string expected = "{";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.OpenBrace()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CloseBraceTest()
        {
            // Arrange
            string expected = "}";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.CloseBrace()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OpenBracketTest()
        {
            // Arrange
            string expected = "[";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.OpenBracket()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CloseBracketTest()
        {
            // Arrange
            string expected = "]";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.CloseBracket()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PlusTest()
        {
            // Arrange
            string expected = "+";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Plus()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PlusEqualTest()
        {
            // Arrange
            string expected = "+=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.PlusEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoublePlusTest()
        {
            // Arrange
            string expected = "++";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoublePlus()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MinusTest()
        {
            // Arrange
            string expected = "-";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Minus()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MinusEqualTest()
        {
            // Arrange
            string expected = "-=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.MinusEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleMinusTest()
        {
            // Arrange
            string expected = "--";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoubleMinus()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CommaTest()
        {
            // Arrange
            string expected = ",";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Comma()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PercentTest()
        {
            // Arrange
            string expected = "%";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Percent()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PercentEqualTest()
        {
            // Arrange
            string expected = "%=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.PercentEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AmpersandTest()
        {
            // Arrange
            string expected = "&";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Ampersand()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleAmpersandTest()
        {
            // Arrange
            string expected = "&&";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoubleAmpersand()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AmpersandEqualTest()
        {
            // Arrange
            string expected = "&=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.AmpersandEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ColonTest()
        {
            // Arrange
            string expected = ":";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Colon()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void QuestionTest()
        {
            // Arrange
            string expected = "?";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Question()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CaretTest()
        {
            // Arrange
            string expected = "^";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Caret()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CaretEqualTest()
        {
            // Arrange
            string expected = "^=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.CaretEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BarTest()
        {
            // Arrange
            string expected = "|";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Bar()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoubleBarTest()
        {
            // Arrange
            string expected = "||";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.DoubleBar()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BarEqualTest()
        {
            // Arrange
            string expected = "|=";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.BarEqual()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DotTest()
        {
            // Arrange
            string expected = ".";

            LinkedList<SyntaxToken> tokens = new(new[]
            {
                SyntaxFactory.Dot()
            });

            // Act
            TokensToTextScaffold scaffold = new(tokens);

            string result = scaffold.GetResult()
                                    .ToString();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
