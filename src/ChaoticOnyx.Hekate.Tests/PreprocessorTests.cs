using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace ChaoticOnyx.Hekate.Tests
{
    public class PreprocessorTests
    {
        private static IImmutableList<SyntaxToken> ParseText(string text)
        {
            CompilationUnit unit = CompilationUnit.FromSource(text);

            return unit.Lexer.Tokens;
        }

        [Fact]
        public void UnknownMacrosDefinitionTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens       = ParseText("#undef macro");
            Preprocessor                preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Issues.Count == 1);

            Assert.True(preprocessor.Issues.First()
                                    .Id
                        == IssuesId.UnknownMacrosDefinition);

            Assert.True(preprocessor.Issues.First()
                                    .Token.Text
                        == "macro");
        }

        [Fact]
        public void MissingEndIfForIfDefTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#ifdef debug
#define macro");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Issues.Count == 1);

            Assert.True(preprocessor.Issues.First()
                                    .Id
                        == IssuesId.EndIfNotFound);
        }

        [Fact]
        public void MissingEndIfForIfNDefTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#ifndef debug
#define macro");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Issues.Count == 1);

            Assert.True(preprocessor.Issues.First()
                                    .Id
                        == IssuesId.EndIfNotFound);
        }

        [Fact]
        public void ExtraEndIf()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#ifdef debug
#define macro
#endif
#endif");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Issues.Count == 1);

            Assert.True(preprocessor.Issues.First()
                                    .Id
                        == IssuesId.ExtraEndIf);
        }

        [Fact]
        public void IncludeTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#include 'code/file1.dm'
#include 'code/file2.dm'");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Includes.Count == 2);

            Assert.True(preprocessor.Includes[0]
                                    .Text
                        == "'code/file1.dm'");

            Assert.True(preprocessor.Includes[1]
                                    .Text
                        == "'code/file2.dm'");
        }

        [Fact]
        public void DefineTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens       = ParseText("#define macro");
            Preprocessor                preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Defines.Count == 1);

            Assert.True(preprocessor.Defines[0]
                                    .Text
                        == "macro");
        }

        [Fact]
        public void DefineAndUndefineTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#define macro
#undef macro");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Defines.Count == 0);
        }

        [Fact]
        public void IfdefDefineTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#define debug
#ifdef debug
#define macro
#endif");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Defines.Count == 2);

            Assert.True(preprocessor.Defines[0]
                                    .Text
                        == "debug");

            Assert.True(preprocessor.Defines[1]
                                    .Text
                        == "macro");
        }

        [Fact]
        public void IfdefUndefTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#define debug
#ifdef debug
#undef debug
#endif");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Defines.Count == 0);
        }

        [Fact]
        public void IfNDefDefineTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#ifndef debug
#define debug
#endif");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Defines.Count == 1);

            Assert.True(preprocessor.Defines[0]
                                    .Text
                        == "debug");
        }

        [Fact]
        public void IfNDefUndefTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#define macro
#ifndef debug
#undef macro
#endif");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Defines.Count == 0);
        }

        [Fact]
        public void ElseTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#ifdef DEBUG
#define DEBUG_DEFINE
#else
#define NOT_DEBUG_DEFINE
#define NOT_DEBUG_DEFINE2
#endif");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Issues.Count == 0);
            Assert.True(preprocessor.Defines.Count == 2);
            Assert.True(preprocessor.Defines[0].Text == "NOT_DEBUG_DEFINE");
            Assert.True(preprocessor.Defines[1].Text == "NOT_DEBUG_DEFINE2");
        }

        [Fact]
        public void NestedIfTest()
        {
            // Arrange
            IImmutableList<SyntaxToken> tokens = ParseText(@"#define TEST
#ifdef DEBUG
#define DEBUG
#else
#ifdef TEST
#define TEST_DEFINE
#endif
#define NOT_DEBUG_DEFINE
#endif");

            Preprocessor preprocessor = Preprocessor.WithTokens(tokens);

            // Act
            preprocessor.Preprocess();

            // Assert
            Assert.True(preprocessor.Issues.Count == 0);
            Assert.True(preprocessor.Defines.Count == 3);
            Assert.True(preprocessor.Defines[0].Text == "TEST");
            Assert.True(preprocessor.Defines[1].Text == "TEST_DEFINE");
            Assert.True(preprocessor.Defines[2].Text == "NOT_DEBUG_DEFINE");
        }
    }
}
