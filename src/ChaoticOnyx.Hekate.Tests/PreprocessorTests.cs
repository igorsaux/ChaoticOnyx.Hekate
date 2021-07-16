using System;
using System.Collections.Generic;
using System.Linq;
using ChaoticOnyx.Hekate.Scaffolds;
using Xunit;

namespace ChaoticOnyx.Hekate.Tests
{
    public class PreprocessorTests
    {
        private static (List<CodeIssue>, LinkedList<SyntaxToken>) ParseText(string text)
        {
            Memory<char>         memory   = new(text.ToCharArray());
            TextToTokensScaffold scaffold = new(memory, new Lexer());

            return scaffold.GetResult();
        }

        [Fact]
        public void MissingEndIfForIfDefTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifdef debug
#define macro")
                .Item2;

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
#define macro")
                .Item2;

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
#endif").Item2;

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
#include 'code/file2.dm'").Item2;

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
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
            LinkedList<SyntaxToken> tokens       = ParseText("#define macro").Item2;
            Preprocessor            preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(context.Defines.Count == 1);
            Assert.True(context.Defines["macro"] == string.Empty);
        }

        [Fact]
        public void DefineAndUndefineTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define macro
#undef macro").Item2;

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.Empty(context.Defines);
        }

        [Fact]
        public void IfdefDefineTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define debug
#ifdef debug
#define macro
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(context.Defines.Count == 2);
            Assert.True(context.Defines["debug"] == string.Empty);
            Assert.True(context.Defines["macro"] == string.Empty);
        }

        [Fact]
        public void IfdefUndefTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define debug
#ifdef debug
#undef debug
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.Empty(context.Defines);
        }

        [Fact]
        public void IfNDefDefineTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifndef debug
#define debug
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.NotEmpty(context.Defines);
            Assert.True(context.Defines["debug"] == string.Empty);
        }

        [Fact]
        public void IfNDefUndefTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define macro
#ifndef debug
#undef macro
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.Empty(context.Defines);
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
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(context.Defines.Count == 2);
            Assert.True(context.Defines["NOT_DEBUG_DEFINE"] == string.Empty);
            Assert.True(context.Defines["NOT_DEBUG_DEFINE2"] == string.Empty);
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
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context = preprocessor.Preprocess(tokens).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(context.Defines.Count == 3);
            Assert.True(context.Defines["TEST"] == string.Empty);
            Assert.True(context.Defines["TEST_DEFINE"] == string.Empty);
            Assert.True(context.Defines["NOT_DEBUG_DEFINE"] == string.Empty);
        }

        [Fact]
        public void ContextTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define TEST
#ifdef TEST
#define DEBUG
#endif").Item2;

            LinkedList<SyntaxToken> tokens2 = ParseText(@"#ifdef DEBUG
#define GOOD
#endif
#undef TEST").Item2;

            Preprocessor preprocessor = new();

            // Act
            PreprocessorContext context1 = preprocessor.Preprocess(tokens).Item2;
            PreprocessorContext context2 = preprocessor.Preprocess(tokens2, context1).Item2;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(context2.Defines.Count == 2);
            Assert.True(context2.Defines["GOOD"] == string.Empty);
        }

        [Fact]
        public void ComplexPreprocessTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifdef TESTING
if(reference_find_on_fail[refID])
	D.find_references()
#ifdef GC_FAILURE_HARD_LOOKUP
else
	D.find_references()
#endif
reference_find_on_fail -= refID
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.Empty(ifs);
            Assert.Empty(defines);
        }

        [Fact]
        public void IfDirectivePreprocessorTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define TEST
#if defined(TEST)
#define PASS
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.Empty(ifs);
            Assert.True(defines.Count == 2);
            Assert.True(defines["TEST"] == string.Empty);
            Assert.True(defines["PASS"] == string.Empty);
        }

        [Fact]
        public void IfDirectiveNegativePreprocessorTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#if !defined(TEST)
#define PASS
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.Empty(ifs);
            Assert.True(defines.Count == 1);
            Assert.True(defines["PASS"] == string.Empty);
        }

        [Fact]
        public void ExpectedValueTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define TEST
#if defined()
#define PASS
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.True(preprocessor.Issues.Count == 1);

            Assert.True(preprocessor.Issues[0]
                                    .Id
                        == IssuesId.ExpectedValue);

            Assert.Empty(ifs);
        }

        [Fact]
        public void DefineValuesTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define TRUE 1
#define FALSE 0").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(defines.ContainsKey("TRUE"));
            Assert.True(defines.ContainsKey("FALSE"));
            Assert.True(defines["TRUE"] == "1");
            Assert.True(defines["FALSE"] == "0");
        }

        [Fact]
        public void EqualityComparisonTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define TRUE 1
#define ANOTHER_TRUE 1
#if TRUE == ANOTHER_TRUE
#define PASS
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(defines.ContainsKey("TRUE"));
            Assert.True(defines.ContainsKey("ANOTHER_TRUE"));
            Assert.True(defines["TRUE"] == "1");
            Assert.True(defines["ANOTHER_TRUE"] == "1");
            Assert.True(defines.ContainsKey("PASS"));
        }

        [Fact]
        public void EqualityComparisonTest2()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define TRUE 1
#define ANOTHER_TRUE 2
#if TRUE == ANOTHER_TRUE
#define PASS
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(defines.ContainsKey("TRUE"));
            Assert.True(defines.ContainsKey("ANOTHER_TRUE"));
            Assert.True(defines["TRUE"] == "1");
            Assert.True(defines["ANOTHER_TRUE"] == "2");
            Assert.True(!defines.ContainsKey("PASS"));
        }

        [Fact]
        public void NotEqualComparisonTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define SOME_VAR 1
#define VAR 2
#if SOME_VAR != VAR
#define PASS
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(defines.ContainsKey("SOME_VAR"));
            Assert.True(defines.ContainsKey("VAR"));
            Assert.True(defines["SOME_VAR"] == "1");
            Assert.True(defines["VAR"] == "2");
            Assert.True(defines.ContainsKey("PASS"));
        }

        [Fact]
        public void NotEqualComparisonTest2()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define SOME_VAR 1
#define VAR 1
#if SOME_VAR != VAR
#define PASS
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(defines.ContainsKey("SOME_VAR"));
            Assert.True(defines.ContainsKey("VAR"));
            Assert.True(defines["SOME_VAR"] == "1");
            Assert.True(defines["VAR"] == "1");
            Assert.True(!defines.ContainsKey("PASS"));
        }

        [Fact]
        public void AlreadyDefinedTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#define SOME_VAR 1
#ifndef TEST
#define SOME_VAR
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.NotEmpty(preprocessor.Issues);

            Assert.True(preprocessor.Issues[0]
                                    .Id
                        == IssuesId.VariableAlreadyDefined);
        }

        [Fact]
        public void IfElseTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifndef DEBUG
#define TEST 0
#define DEBUG
#else
#define TEST 1
#warning Test is 1
#endif").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(defines.Count == 2);
            Assert.True(defines["TEST"] == "0");
            Assert.True(defines.ContainsKey("DEBUG"));
        }

        [Fact]
        public void WarningDirectiveTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens       = ParseText("#warning This is a warning").Item2;
            Preprocessor            preprocessor = new();

            // Act
            var _ = preprocessor.Preprocess(tokens, new PreprocessorContext());

            // Assert
            Assert.NotEmpty(preprocessor.Issues);
            Assert.True(preprocessor.Issues[0].Id == IssuesId.WarningDirective);
        }

        [Fact]
        public void ErrorDirectiveTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens       = ParseText("#error This is a error").Item2;
            Preprocessor            preprocessor = new();

            // Act
            var _ = preprocessor.Preprocess(tokens, new PreprocessorContext());

            // Assert
            Assert.NotEmpty(preprocessor.Issues);
            Assert.True(preprocessor.Issues[0].Id == IssuesId.ErrorDirective);
        }

        [Theory]
        [InlineData(1, "==", 1)]
        [InlineData(1, "!=", 2)]
        [InlineData(2, ">", 1)]
        [InlineData(1, ">=", 1)]
        [InlineData(5, ">=", 1)]
        [InlineData(1, "<", 2)]
        [InlineData(1, "<=", 1)]
        [InlineData(1, "<=", 5)]
        public void ComparisonTest(int lvalue, string op, int rvalue)
        {
            // Arrange
            LinkedList<SyntaxToken> tokens       = ParseText($@"#if {lvalue} {op} {rvalue}
#define PASS
#endif").Item2;
            Preprocessor            preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(defines.Count == 1);
            Assert.True(defines.ContainsKey("PASS"));
        }

        [Fact]
        public void DefineProcTest()
        {
            // Arrange
            LinkedList<SyntaxToken> tokens = ParseText(@"#ifdef T_BOARD
#error T_BOARD already defined elsewhere, we can't use it.
#endif
#define T_BOARD(name)	""circuit board ("" + (name) + "")""").Item2;

            Preprocessor preprocessor = new();

            // Act
            var (_, context)      = preprocessor.Preprocess(tokens, new PreprocessorContext());
            var (_, defines, ifs) = context;

            // Assert
            Assert.Empty(preprocessor.Issues);
            Assert.True(defines.Count == 1);
            Assert.True(defines.ContainsKey("T_BOARD"));
        }
    }
}
