#region

using System;
using System.Collections.Generic;
using System.Linq;
using ChaoticOnyx.Hekate.Scaffolds;
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
            ReadOnlyMemory<char> text = new(@"/* MultiLine Comment*/

// SingleLine Comment".ToCharArray());

            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;

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

        [Theory]
        [InlineData("literal")]
        [InlineData("_someIdentifier")]
        [InlineData("_some_underscored_identifier")]
        public void IdentifierParsing(string expected)
        {
            // Arrange
            ReadOnlyMemory<char> text     = new(expected.ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count: 2 });
            SyntaxToken token = tokens.First!.Value!;
            Assert.NotNull(tokens);
            Assert.True(token is { Kind: SyntaxKind.Identifier });
            Assert.True(token.Text == expected);
        }

        [Fact]
        public void NumericalLiteralParsing()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("123".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;

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
            ReadOnlyMemory<char> text     = new("123.55".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count: 2 });
            SyntaxToken token = tokens.First!.Value!;
            Assert.NotNull(token);
            Assert.True(token is { Kind: SyntaxKind.NumericalLiteral });
            Assert.True(token is { Text: "123.55" });
        }

        [Theory]
        [InlineData("\"TextLiteral\"")]
        [InlineData("\"\\t\"")]
        [InlineData("\"#\"")]
        [InlineData("\"\\n\"")]
        public void TextLiteralParsing(string expected)
        {
            // Arrange
            ReadOnlyMemory<char> text     = new(expected.ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            LinkedList<SyntaxToken> tokens = scaffold.GetResult();

            // Assert
            Assert.NotEmpty(tokens);
            Assert.Empty(scaffold.Lexer.Issues);
            Assert.True(tokens is { Count: 2 });
            SyntaxToken token = tokens.First!.Value!;
            Assert.NotNull(token);
            Assert.True(token is { Kind: SyntaxKind.TextLiteral });
            Assert.Equal(token.Text, expected);
        }

        [Theory]
        [InlineData("@\"Hello!\"")]
        [InlineData("@1Hello!1")]
        [InlineData("@|Hello!|")]
        [InlineData("@NHello!N")]
        public void RawTextLiteralParsing(string expected)
        {
            // Arrange
            ReadOnlyMemory<char> text     = new(expected.ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            LinkedList<SyntaxToken> tokens = scaffold.GetResult();

            // Assert
            Assert.NotEmpty(tokens);
            Assert.Empty(scaffold.Lexer.Issues);
            SyntaxToken token = tokens.First!.Value;
            Assert.True(token.Kind == SyntaxKind.TextLiteral);
            Assert.Equal(token.Text, expected);
        }

        [Fact]
        public void PathLiteralParsing()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("\'PathLiteral/file.dm\'".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;

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
            ReadOnlyMemory<char> text     = new(@"    // Comment".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;

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
            ReadOnlyMemory<char> text     = new("#include #ifndef #ifdef #endif #define #undef".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;

            // Assert
            Assert.Empty(scaffold.Lexer.Issues);
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count: 7 });
            Assert.True(tokens.Count(t => t.Kind == kind) == 1);
        }

        [Fact]
        public void WarningDirectiveParsing()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("#warning This is a warning".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            List<SyntaxToken> tokens = scaffold.Lexer.Tokens.ToList();

            // Assert
            Assert.Empty(scaffold.Lexer.Issues);
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count  : 3 });
            Assert.True(tokens[0] is { Kind: SyntaxKind.WarningDirective });
            Assert.True(tokens[1] is { Kind: SyntaxKind.TextLiteral, Text: "This is a warning" });
        }

        [Fact]
        public void ErrorDirectiveParsing()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("#error This is a error".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            List<SyntaxToken> tokens = scaffold.Lexer.Tokens.ToList();

            // Assert
            Assert.Empty(scaffold.Lexer.Issues);
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count  : 3 });
            Assert.True(tokens[0] is { Kind: SyntaxKind.ErrorDirective });
            Assert.True(tokens[1] is { Kind: SyntaxKind.TextLiteral, Text: "This is a error" });
        }

        [Fact]
        public void IfDirectiveParsing()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("#if defined(TEST)".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            List<SyntaxToken> tokens = scaffold.Lexer.Tokens.ToList();

            // Assert
            Assert.Empty(scaffold.Lexer.Issues);
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count  : 6 });
            Assert.True(tokens[0] is { Kind: SyntaxKind.IfDirective });
            Assert.True(tokens[1] is { Kind: SyntaxKind.Identifier, Text: "defined" });
            Assert.True(tokens[3] is { Kind: SyntaxKind.Identifier, Text: "TEST" });
        }

        [Fact]
        public void ElseDirectiveParsing()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("#else".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            List<SyntaxToken> tokens = scaffold.Lexer.Tokens.ToList();

            // Assert
            Assert.Empty(scaffold.Lexer.Issues);
            Assert.NotEmpty(tokens);
            Assert.True(tokens is { Count  : 2 });
            Assert.True(tokens[0] is { Kind: SyntaxKind.ElseDirective });
        }

        [Fact]
        public void ConcatDirectiveParsing()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("#define TEST(X) ##x".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;

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
            ReadOnlyMemory<char> text     = new("for new global throw catch try var verb proc in if else set as while".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;

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
        [InlineData(SyntaxKind.At)]
        public void CheckTokenParsing(SyntaxKind kind, int expectedCount = 1)
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("* *= \\= '' \"\" / == = =!!= >= > >> >>= <= < << <<= () {} [] + ++ += - -- -=,, ** & &=&& /= % %= : ? ^ ^= | |= || \\ . ; @".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            scaffold.GetResult();
            LinkedList<SyntaxToken> tokens = scaffold.Lexer.Tokens;
            int                     count  = tokens.Count(token => token.Kind == kind);

            // Assert
            Assert.NotEmpty(tokens);
            Assert.Empty(scaffold.Lexer.Issues);
            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public void ParseEscapedText()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("\"N[pick(\"'\",\"`\")]ath reth sh'yro eth d'raggathnor!\"".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            List<SyntaxToken> tokens = scaffold.GetResult()
                                               .ToList();

            // Assert
            Assert.NotEmpty(tokens);
            Assert.Empty(scaffold.Lexer.Issues);
            Assert.True(tokens.Count == 2);

            Assert.True(tokens[0]
                            .Kind
                        == SyntaxKind.TextLiteral);

            Assert.True(tokens[1]
                            .Kind
                        == SyntaxKind.EndOfFile);
        }

        [Fact]
        public void ParseDocumentText()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("{\"\"[txt]\"\"}".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            List<SyntaxToken> tokens = scaffold.GetResult()
                                               .ToList();

            // Assert
            Assert.NotEmpty(tokens);
            Assert.True(tokens.Count == 2);

            Assert.True(tokens[0]
                            .Kind
                        == SyntaxKind.TextLiteral);

            Assert.True(tokens[1]
                            .Kind
                        == SyntaxKind.EndOfFile);
        }

        [Fact]
        public void ParseEscapedText2()
        {
            // Arange
            ReadOnlyMemory<char> text = new(@"/proc/pencode2html(t)
t = replacetext(t, ""\n"", ""<BR>"")
t = replacetext(t, ""\[center\]"", ""<center>"")
t = replacetext(t, ""\[/center\]"", ""</center>"")
t = replacetext(t, ""\[right\]"", ""<div style=\""text-align:right\"">"")
t = replacetext(t, ""\[/right\]"", ""</div>"")
t = replacetext(t, ""\[left\]"", ""<div style=\""text-align:left\"">"")
t = replacetext(t, ""\[/left\]"", ""</div>"")
t = replacetext(t, ""\[br\]"", ""<BR>"")
t = replacetext(t, ""\[b\]"", ""<B>"")
t = replacetext(t, ""\[/b\]"", ""</B>"")
t = replacetext(t, ""\[i\]"", ""<I>"")
t = replacetext(t, ""\[/i\]"", ""</I>"")
t = replacetext(t, ""\[u\]"", ""<U>"")
t = replacetext(t, ""\[/u\]"", ""</U>"")
t = replacetext(t, ""\[time\]"", ""[stationtime2text()]"")
t = replacetext(t, ""\[date\]"", ""[stationdate2text()]"")
t = replacetext(t, ""\[large\]"", ""<font size=\""4\"">"")
t = replacetext(t, ""\[/large\]"", ""</font>"")
t = replacetext(t, ""\[field\]"", ""<!--paper_field-->"")
t = replacetext(t, ""\[h1\]"", ""<H1>"")
t = replacetext(t, ""\[/h1\]"", ""</H1>"")
t = replacetext(t, ""\[h2\]"", ""<H2>"")
t = replacetext(t, ""\[/h2\]"", ""</H2>"")
t = replacetext(t, ""\[h3\]"", ""<H3>"")
t = replacetext(t, ""\[/h3\]"", ""</H3>"")
t = replacetext(t, ""\[*\]"", ""<li>"")
t = replacetext(t, ""\[hr\]"", ""<HR>"")
t = replacetext(t, ""\[small\]"", ""<font size = \""1\"">"")
t = replacetext(t, ""\[/small\]"", ""</font>"")
t = replacetext(t, ""\[medium\]"", ""<font size = \""2\"">"")
t = replacetext(t, ""\[/medium\]"", ""</font>"")
t = replacetext(t, ""\[list\]"", ""<ul>"")
t = replacetext(t, ""\[/list\]"", ""</ul>"")
t = replacetext(t, ""\[item\]"", ""<li>"")
t = replacetext(t, ""\[/item\]"", ""</li>"")
t = replacetext(t, ""\[ord\]"", ""<ol>"")
t = replacetext(t, ""\[/ord\]"", ""</ol>"")
t = replacetext(t, ""\[table\]"", ""<table border=1 cellspacing=0 cellpadding=3 style='border: 1px solid black;'>"")
t = replacetext(t, ""\[/table\]"", ""</td></tr></table>"")
t = replacetext(t, ""\[grid\]"", ""<table>"")
t = replacetext(t, ""\[/grid\]"", ""</td></tr></table>"")
t = replacetext(t, ""\[row\]"", ""</td><tr>"")
t = replacetext(t, ""\[cell\]"", ""<td>"")
t = replacetext(t, ""\[logo\]"", ""<img src = ntlogo.png>"")
t = replacetext(t, ""\[bluelogo\]"", ""<img src = bluentlogo.png>"")
t = replacetext(t, ""\[solcrest\]"", ""<img src = sollogo.png>"")
t = replacetext(t, ""\[terraseal\]"", ""<img src = terralogo.png>"")
t = replacetext(t, ""\[editorbr\]"", """")
return t".ToCharArray());

            TextToTokensScaffold scaffold = new(text);

            // Act
            List<SyntaxToken> tokens = scaffold.GetResult()
                                               .ToList();

            // Assert
            Assert.NotEmpty(tokens);
            Assert.Empty(scaffold.Lexer.Issues);
        }

        [Fact]
        public void ParseEscapedText3()
        {
            // Arrange
            ReadOnlyMemory<char> text     = new("var/static/list/json_escape = list(\"\\\\\" = \"\\\\\\\\\", \"\\\"\" = \"\\\\\\\"\", \"\\n\" = \"\\\\n\")+rus_unicode_conversion".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            List<SyntaxToken> tokens = scaffold.GetResult()
                                               .ToList();

            // Assert
            Assert.NotEmpty(tokens);
            Assert.Empty(scaffold.Lexer.Issues);
            Assert.True(tokens.Count == 25);
        }

        [Fact]
        public void RawRegexStringParsingTest()
        {
            // Arrange
            ReadOnlyMemory<char> text = new(@"regex(@{""([^\u0020-\u8000]+)""})
	for(var/i in buttons)".ToCharArray());

            TextToTokensScaffold scaffold = new(text);

            // Act
            List<SyntaxToken> tokens = scaffold.GetResult()
                                               .ToList();

            // Assert
            Assert.Empty(scaffold.Lexer.Issues);
        }
    }
}
