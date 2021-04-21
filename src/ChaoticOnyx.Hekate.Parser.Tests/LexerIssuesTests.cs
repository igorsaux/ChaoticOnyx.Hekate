#region

using System.Collections.Generic;
using System.Linq;
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
            // Act
            CompilationUnit                unit   = CompilationUnit.FromSource("\'");
            IReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors.First()
                              .Id
                        == IssuesId.MissingClosingSign);
        }

        [Fact]
        public void Dm0001ErrorDoubleQuote()
        {
            // Arrange
            // Act
            CompilationUnit                unit   = CompilationUnit.FromSource("\"");
            IReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors.First()
                              .Id
                        == IssuesId.MissingClosingSign);
        }

        [Fact]
        public void Dm0001ErrorMultiLineComment()
        {
            // Arrange
            // Act
            CompilationUnit                unit   = CompilationUnit.FromSource("/* Comment without end *");
            IReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors.First()
                              .Id
                        == IssuesId.MissingClosingSign);
        }

        [Fact]
        public void Dm0002Error()
        {
            // Arrange
            // Act
            CompilationUnit                unit   = CompilationUnit.FromSource("$token");
            IReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors.First()
                              .Id
                        == IssuesId.UnexpectedToken);
        }

        [Fact]
        public void Dm0003Error()
        {
            // Arrange
            // Act
            CompilationUnit                unit   = CompilationUnit.FromSource("#pragma");
            IReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors.First()
                              .Id
                        == IssuesId.UnknownDirective);
        }
    }
}
