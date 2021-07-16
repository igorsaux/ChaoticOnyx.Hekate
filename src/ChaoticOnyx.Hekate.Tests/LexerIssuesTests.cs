#region

using System;
using System.Collections.Generic;
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
            ReadOnlyMemory<char>  text     = new("\'".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            List<CodeIssue> errors = scaffold.GetResult()
                                             .Item1;

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
            ReadOnlyMemory<char>  text     = new("\"".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            List<CodeIssue> errors = scaffold.GetResult()
                                             .Item1;

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
            ReadOnlyMemory<char>  text     = new("/* Comment without end *".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            List<CodeIssue> errors = scaffold.GetResult()
                                             .Item1;

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
            ReadOnlyMemory<char>  text     = new("$token".ToCharArray());
            TextToTokensScaffold scaffold = new(text);

            // Act
            List<CodeIssue> errors = scaffold.GetResult()
                                             .Item1;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id
                        == IssuesId.UnexpectedToken);
        }
    }
}
