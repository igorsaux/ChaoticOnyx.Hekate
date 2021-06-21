#region

using System.Collections.Immutable;
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
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("\'");
            unit.Parse();
            IImmutableList<CodeIssue> errors = unit.Lexer.Issues;

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
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("\"");
            unit.Parse();
            IImmutableList<CodeIssue> errors = unit.Lexer.Issues;

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
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("/* Comment without end *");
            unit.Parse();
            IImmutableList<CodeIssue> errors = unit.Lexer.Issues;

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
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("$token");
            unit.Parse();
            IImmutableList<CodeIssue> errors = unit.Lexer.Issues;

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
            // Act
            CompilationUnit unit = CompilationUnit.FromSource("#pragma");
            unit.Parse();
            IImmutableList<CodeIssue> errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id
                        == IssuesId.UnknownDirective);
        }
    }
}
