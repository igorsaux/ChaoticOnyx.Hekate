#region

using System.Collections.Generic;
using System.Linq;
using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Tests
{
    public class LexerParsingTests
    {
        [Fact]
        public void CommentParsing()
        {
            // Arrange
            // Act
            CompilationUnit unit = CompilationUnit.FromSource(@"/* MultiLine Comment*/

// SingleLine Comment");

            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.Single(tokens);
            SyntaxToken token = tokens.First?.Value!;
            Assert.NotNull(token);
            Assert.True(token is { Kind: SyntaxKind.EndOfFile });
            Assert.NotEmpty(token.Leads);
            Assert.True(token.Leads is { Count: 4 });
            LinkedListNode<SyntaxToken> firstLead = token.Leads.First!;
            LinkedListNode<SyntaxToken> lastLead  = token.Leads.Last!;
            Assert.NotNull(firstLead);
            Assert.NotNull(lastLead);
            Assert.True(firstLead!.Value is { Kind: SyntaxKind.MultiLineComment });
            Assert.True(token.Leads.Count(t => t.Kind == SyntaxKind.EndOfLine) == 2);
            Assert.True(lastLead!.Value is { Kind: SyntaxKind.SingleLineComment });
        }

        [Fact]
        public void IdentifierParsing()
        {
            // Arrange
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("literal");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count: 2 });
            SyntaxToken token = tokens.First!.Value!;
            Assert.NotNull(tokens);
            Assert.True(token is { Kind: SyntaxKind.Identifier });
            Assert.True(token is { Text: "literal" });
        }

        [Fact]
        public void NumericalLiteralParsing()
        {
            // Arrange
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("123");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count: 2 });
            SyntaxToken token = tokens.First!.Value!;
            Assert.NotNull(token);
            Assert.True(token is { Kind: SyntaxKind.NumericalLiteral });
            Assert.True(token is { Text: "123" });
        }

        [Fact]
        public void FloatNumericalLiteralParsing()
        {
            // Arrange
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("123.55");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count: 2 });
            SyntaxToken token = tokens.First!.Value!;
            Assert.NotNull(token);
            Assert.True(token is { Kind: SyntaxKind.NumericalLiteral });
            Assert.True(token is { Text: "123.55" });
        }

        [Fact]
        public void TextLiteralParsing()
        {
            // Arrange
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("\"TextLiteral\"");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count: 2 });
            SyntaxToken token = tokens.First!.Value!;
            Assert.NotNull(token);
            Assert.True(token is { Kind: SyntaxKind.TextLiteral });
        }

        [Fact]
        public void PathLiteralParsing()
        {
            // Arrange
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("\'PathLiteral/file.dm\'");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count: 2 });
            SyntaxToken token = tokens.First!.Value!;
            Assert.NotNull(token);
            Assert.True(token is { Kind: SyntaxKind.PathLiteral });
        }

        [Fact]
        public void SpacesParsing()
        {
            // Arrange
            // Act
            CompilationUnit unit = CompilationUnit.FromSource(@"    // Comment");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens.Count == 1);
            SyntaxToken token = tokens.First!.Value!;
            Assert.NotNull(token);
            Assert.True(token.Leads.Count == 2);
            SyntaxToken lead1 = token.Leads.First!.Value!;
            SyntaxToken lead2 = token.Leads.First.Next!.Value!;
            Assert.NotNull(lead1);
            Assert.NotNull(lead2);
            Assert.True(lead1 is { Kind: SyntaxKind.WhiteSpace });
            Assert.True(lead2 is { Kind: SyntaxKind.SingleLineComment });
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
            CompilationUnit unit = CompilationUnit.FromSource("#include #ifndef #ifdef #endif #define #undef");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count: 7 });
            Assert.True(tokens.Count(t => t.Kind == kind) == 1);
        }

        [Fact]
        public void ConcatDirectiveParsing()
        {
            // Arrange
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("#define TEST(X) ##x");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens.Count == 8);
            SyntaxToken lastLead    = tokens.Last!.Previous!.Value!;
            SyntaxToken preLastLead = tokens.Last.Previous.Previous!.Value!;
            Assert.NotNull(lastLead);
            Assert.NotNull(preLastLead);
            Assert.True(lastLead is { Kind   : SyntaxKind.Identifier });
            Assert.True(preLastLead is { Kind: SyntaxKind.ConcatDirective });
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
            CompilationUnit unit = CompilationUnit.FromSource("for new global throw catch try var verb proc in if else set as while");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
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
        [InlineData(SyntaxKind.Unknown, 0)]
        [InlineData(SyntaxKind.PathLiteral)]
        [InlineData(SyntaxKind.TextLiteral)]
        [InlineData(SyntaxKind.SlashEqual)]
        [InlineData(SyntaxKind.Slash)]
        [InlineData(SyntaxKind.Semicolon)]
        [InlineData(SyntaxKind.Backslash)]
        [InlineData(SyntaxKind.BackSlashEqual)]
        public void CheckTokenParsing(SyntaxKind kind, int expectedCount = 1)
        {
            // Arrange
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("* *= \\= '' \"\" / == = =!!= >= > >> >>= <= < << <<= () {} [] + ++ += - -- -=,, ** & &=&& /= % %= : ? ^ ^= | |= || \\ . ;");
            unit.Parse();
            LinkedList<SyntaxToken> tokens = unit.Tokens;
            int                     count  = tokens.Count(token => token.Kind == kind);

            // Assert
            Assert.NotEmpty(tokens);
            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public void EscapedTextTest()
        {
            // Arrange
            const string Text = @"var/a = ""chemical_reactions_list\[\""[reaction]\""\] = \""[chemical_reactions_list[reaction]]\""\n""";

            // Act
            CompilationUnit unit = CompilationUnit.FromSource(Text);
            unit.Parse();
            List<SyntaxToken> tokens = unit.Tokens.ToList();

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens.Count == 6);

            Assert.True(tokens[0]
                            .Kind
                        == SyntaxKind.VarKeyword);

            Assert.True(tokens[1]
                            .Kind
                        == SyntaxKind.Slash);

            Assert.True(tokens[2]
                            .Kind
                        == SyntaxKind.Identifier);

            Assert.True(tokens[3]
                            .Kind
                        == SyntaxKind.Equal);

            Assert.True(tokens[4]
                            .Kind
                        == SyntaxKind.TextLiteral);

            Assert.True(tokens[5]
                            .Kind
                        == SyntaxKind.EndOfFile);
        }
    }
}
