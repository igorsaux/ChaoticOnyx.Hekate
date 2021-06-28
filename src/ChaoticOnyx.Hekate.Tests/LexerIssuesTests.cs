#region

using System;
using System.Collections.Immutable;
using System.Linq;
using ChaoticOnyx.Hekate.Scaffolds;
using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Tests
{
    public class LexerIssuesTests
    {
        [Fact]
        public void Dm0001SingleQuote()
        {
            // Arrange
            var text     = new ReadOnlyMemory<char>("\'".ToCharArray());
            var scaffold = new TextToTokensScaffold(text);

            // Act
            scaffold.GetResult();
            var errors = scaffold.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id
                        == IssuesId.MissingClosingSign);
        }

        [Fact]
        public void Dm0001DoubleQuote()
        {
            // Arrange
            var text     = new ReadOnlyMemory<char>("\"".ToCharArray());
            var scaffold = new TextToTokensScaffold(text);

            // Act
            scaffold.GetResult();
            var errors = scaffold.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id
                        == IssuesId.MissingClosingSign);
        }

        [Fact]
        public void Dm0001MultiLineComment()
        {
            // Arrange
            var text     = new ReadOnlyMemory<char>("/* Comment without end *".ToCharArray());
            var scaffold = new TextToTokensScaffold(text);

            // Act
            scaffold.GetResult();
            var errors = scaffold.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id
                        == IssuesId.MissingClosingSign);
        }

        [Fact]
        public void Dm0002UnexpectedToken()
        {
            // Arrange
            var text     = new ReadOnlyMemory<char>("$token".ToCharArray());
            var scaffold = new TextToTokensScaffold(text);

            // Act
            scaffold.GetResult();
            var errors = scaffold.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id
                        == IssuesId.UnexpectedToken);
        }

        [Fact]
        public void Dm0003UnknownDirective()
        {
            // Arrange
            var text     = new ReadOnlyMemory<char>("#pragma".ToCharArray());
            var scaffold = new TextToTokensScaffold(text);

            // Act
            scaffold.GetResult();
            var errors = scaffold.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id
                        == IssuesId.UnknownDirective);
        }
    }
}
