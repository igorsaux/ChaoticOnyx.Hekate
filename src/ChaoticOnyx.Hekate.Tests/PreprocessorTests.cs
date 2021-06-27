using System.Collections.Generic;
using Xunit;

namespace ChaoticOnyx.Hekate.Tests
{
    public class PreprocessorTests
    {
        private static LinkedList<SyntaxToken> ParseText(string text)
        {
            CompilationUnit unit = CompilationUnit.FromSource(text);
            unit.Parse(PreprocessorContext.Empty);

            return unit.Tokens;
        }

        [Fact]
        public void UnknownMacrosDefinitionTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens       = ParseText("#undef macro");
            Preprocessor            preprocessor = new();

            // Act
            preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(preprocessor.Issues.Count == 1);

            Assert.True(preprocessor.Issues[0]
                                    .Id
                        == IssuesId.UnknownMacrosDefinition);

            Assert.True(preprocessor.Issues[0]
                                    .Token.Text
                        == "macro");
        }

        [Fact]
        public void MissingEndIfForIfDefTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifdef debug
#define macro");

            Preprocessor preprocessor = new();

            // Act
            preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(preprocessor.Issues.Count == 1);

            Assert.True(preprocessor.Issues[0]
                                    .Id
                        == IssuesId.EndIfNotFound);
        }

        [Fact]
        public void MissingEndIfForIfNDefTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifndef debug
#define macro");

            Preprocessor preprocessor = new();

            // Act
            preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(preprocessor.Issues.Count == 1);

            Assert.True(preprocessor.Issues[0]
                                    .Id
                        == IssuesId.EndIfNotFound);
        }

        [Fact]
        public void ExtraEndIf()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifdef debug
#define macro
#endif
#endif");

            Preprocessor preprocessor = new();

            // Act
            preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(preprocessor.Issues.Count == 1);

            Assert.True(preprocessor.Issues[0]
                                    .Id
                        == IssuesId.ExtraEndIf);
        }

        [Fact]
        public void IncludeTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#include 'code/file1.dm'
#include 'code/file2.dm'");

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(context.Includes.Count == 2);

            Assert.True(context.Includes[0]
                               .Text
                        == "'code/file1.dm'");

            Assert.True(context.Includes[1]
                               .Text
                        == "'code/file2.dm'");
        }

        [Fact]
        public void DefineTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens       = ParseText("#define macro");
            Preprocessor            preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(context.Defines.Count == 1);

            Assert.True(context.Defines[0]
                               .Text
                        == "macro");
        }

        [Fact]
        public void DefineAndUndefineTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define macro
#undef macro");

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(context.Defines.Count == 0);
        }

        [Fact]
        public void IfdefDefineTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define debug
#ifdef debug
#define macro
#endif");

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(context.Defines.Count == 2);

            Assert.True(context.Defines[0]
                               .Text
                        == "debug");

            Assert.True(context.Defines[1]
                               .Text
                        == "macro");
        }

        [Fact]
        public void IfdefUndefTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define debug
#ifdef debug
#undef debug
#endif");

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(context.Defines.Count == 0);
        }

        [Fact]
        public void IfNDefDefineTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifndef debug
#define debug
#endif");

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(context.Defines.Count == 1);

            Assert.True(context.Defines[0]
                               .Text
                        == "debug");
        }

        [Fact]
        public void IfNDefUndefTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define macro
#ifndef debug
#undef macro
#endif");

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(context.Defines.Count == 0);
        }

        [Fact]
        public void ElseTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifdef DEBUG
#define DEBUG_DEFINE
#else
#define NOT_DEBUG_DEFINE
#define NOT_DEBUG_DEFINE2
#endif");

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(preprocessor.Issues.Count == 0);
            Assert.True(context.Defines.Count == 2);

            Assert.True(context.Defines[0]
                               .Text
                        == "NOT_DEBUG_DEFINE");

            Assert.True(context.Defines[1]
                               .Text
                        == "NOT_DEBUG_DEFINE2");
        }

        [Fact]
        public void NestedIfTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define TEST
#ifdef DEBUG
#define DEBUG
#else
#ifdef TEST
#define TEST_DEFINE
#endif
#define NOT_DEBUG_DEFINE
#endif");

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens);

            // Assert
            Assert.True(preprocessor.Issues.Count == 0);
            Assert.True(context.Defines.Count == 3);

            Assert.True(context.Defines[0]
                               .Text
                        == "TEST");

            Assert.True(context.Defines[1]
                               .Text
                        == "TEST_DEFINE");

            Assert.True(context.Defines[2]
                               .Text
                        == "NOT_DEBUG_DEFINE");
        }

        [Fact]
        public void ContextTest()
        {
            LinkedList<SyntaxToken> tokens = ParseText(@"#define TEST
#ifdef TEST
#define DEBUG
#endif");

            LinkedList<SyntaxToken> tokens2 = ParseText(@"#ifdef DEBUG
#define GOOD
#endif
#undef TEST");

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context1 = preprocessor.Preprocess(tokens);
            PreprocessorContext context2 = preprocessor.Preprocess(tokens2, context1);

            // Assert
            Assert.True(preprocessor.Issues.Count == 0);
            Assert.True(context2.Defines.Count == 2);

            Assert.True(context2.Defines[1]
                                .Text
                        == "GOOD");
        }
    }
}
