#region

using System.Collections.Generic;
using System.Linq;
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
            CompilationUnit                unit   = CompilationUnit.FromSource("\'");
            IReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

            // Assert
            Assert.True(errors.Count == 1);

            Assert.True(errors.First()
                              .Id
                        == IssuesId.MissingClosingSign);
        }

        [Fact]
        public void Dm0001DoubleQuote()
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
        public void Dm0001MultiLineComment()
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
        public void Dm0002UnexpectedToken()
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
        public void Dm0003UnknownDirective()
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
