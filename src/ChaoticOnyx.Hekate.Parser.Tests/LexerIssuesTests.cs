#region

using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Parser.Tests
{
    public class LexerIssuesTests
    {
        [Fact]
        public void Dm0001ErrorSingleQuote()
        {
            // Arrange
            CompilationUnit unit = new("\'");

            // Act
            unit.Parse();
            var errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id == IssueId.Dm0001);
        }

        [Fact]
        public void Dm0001ErrorDoubleQuote()
        {
            // Arrange
            CompilationUnit unit = new("\"");

            // Act
            unit.Parse();
            var errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id == IssueId.Dm0001);
        }

        [Fact]
        public void Dm0001ErrorMultiLineComment()
        {
            // Arrange
            CompilationUnit unit = new("/* Comment without end *");

            // Act
            unit.Parse();
            var errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id == IssueId.Dm0001);
        }

        [Fact]
        public void Dm0002Error()
        {
            // Arrange
            CompilationUnit unit = new("$token");

            // Act
            unit.Parse();
            var errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id == IssueId.Dm0002);
        }

        [Fact]
        public void Dm0003Error()
        {
            // Arrange
            CompilationUnit unit = new("#pragma");

            // Act
            unit.Parse();
            var errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors[0]
                            .Id == IssueId.Dm0003);
        }
    }
}
