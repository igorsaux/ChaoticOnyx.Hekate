#region

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
            CompilationUnit unit = new(@"/* MultiLine Comment*/

// SingleLine Comment");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;
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
            CompilationUnit unit = new("literal");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;

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
            CompilationUnit unit = new("123");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;

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
            CompilationUnit unit = new("123.55");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;

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
            CompilationUnit unit = new("\"TextLiteral\"");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;

            // Assert
            Assert.True(tokens.Count == 2);

            Assert.True(tokens[0]
                            .Kind == SyntaxKind.TextLiteral);
        }

        [Fact]
        public void PathLiteralParsing()
        {
            // Arrange
            CompilationUnit unit = new("\'PathLiteral/file.dm\'");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;

            // Assert
            Assert.True(tokens.Count == 2);

            Assert.True(tokens[0]
                            .Kind == SyntaxKind.PathLiteral);
        }

        [Fact]
        public void SpacesParsing()
        {
            // Arrange
            CompilationUnit unit = new(@"    // Comment");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;

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
        public void DirectiveParsing(SyntaxKind kind)
        {
            // Arrange
            CompilationUnit unit = new("#include #ifndef #ifdef #endif #define");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;

            // Assert
            Assert.True(tokens.Count == 6);
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
            CompilationUnit unit = new("for new global throw catch try var verb proc in if else set as while");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;

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
            CompilationUnit unit =
                new(
                    "* *= \\= '' \"\" / == = =!!= >= > >> >>= <= < << <<= () {} [] + ++ += - -- -=,, ** & &=&& /= % %= : ? ^ ^= | |= || \\ .");

            // Act
            unit.Parse();
            var tokens = unit.Lexer.Tokens;
            var count  = tokens.Count(token => token.Kind == kind);

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
            var unit = new CompilationUnit(text, tabSize);
            var tabs = text.Count(c => c == '\t');

            // Act
            unit.Parse();
            var token = unit.Lexer.Tokens[0];

            // Assert
            Assert.True(token.FilePosition.Column == 1 + tabs * tabSize);
        }
    }
}
