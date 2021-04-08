using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChaoticOnyx.Hekate.Parser.Tests
{
	public class PreprocessorTests
	{
		private static IList<SyntaxToken> ParseText(string text)
		{
			var unit = new CompilationUnit(text);
			unit.Parse();

			return unit.Lexer.Tokens;
		}
		
		[Fact]
		public void UnknownMacrosDefinitionTest()
		{
			// Arrange
			IList<SyntaxToken> tokens       = ParseText("#undef macro");
			var                preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Issues.Count == 1);

			Assert.True(preprocessor.Issues.First()
									.Id == IssuesId.UnknownMacrosDefinition);

			Assert.True(preprocessor.Issues.First()
									.Token.Text == "macro");
		}

		[Fact]
		public void MissingEndIfForIfDefTest()
		{
			// Arrange
			IList<SyntaxToken> tokens = ParseText(@"#ifdef debug
#define macro");

			var preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Issues.Count == 1);

			Assert.True(preprocessor.Issues.First()
									.Id == IssuesId.EndIfNotFound);
		}

		[Fact]
		public void MissingEndIfForIfNDefTest()
		{
			// Arrange
			IList<SyntaxToken> tokens = ParseText(@"#ifndef debug
#define macro");

			var preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Issues.Count == 1);

			Assert.True(preprocessor.Issues.First()
									.Id == IssuesId.EndIfNotFound);
		}

		[Fact]
		public void ExtraEndIf()
		{
			// Arrange
			IList<SyntaxToken> tokens = ParseText(@"#ifdef debug
#define macro
#endif
#endif");

			var preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Issues.Count == 1);

			Assert.True(preprocessor.Issues.First()
									.Id == IssuesId.ExtraEndIf);
		}

		[Fact]
		public void IncludeTest()
		{
			// Arrange
			IList<SyntaxToken> tokens = ParseText(@"#include 'code/file1.dm'
#include 'code/file2.dm'");

			var preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Includes.Count == 2);

			Assert.True(preprocessor.Includes[0]
									.Text == "'code/file1.dm'");

			Assert.True(preprocessor.Includes[1]
									.Text == "'code/file2.dm'");
		}

		[Fact]
		public void DefineTest()
		{
			// Arrange
			IList<SyntaxToken> tokens       = ParseText("#define macro");
			var                preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Defines.Count == 1);

			Assert.True(preprocessor.Defines[0]
									.Text == "macro");
		}

		[Fact]
		public void DefineAndUndefineTest()
		{
			// Arrange
			IList<SyntaxToken> tokens = ParseText(@"#define macro
#undef macro");

			var preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Defines.Count == 0);
		}

		[Fact]
		public void IfdefDefineTest()
		{
			// Arrange
			IList<SyntaxToken> tokens = ParseText(@"#define debug
#ifdef debug
#define macro
#endif");

			var preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Defines.Count == 2);

			Assert.True(preprocessor.Defines[0]
									.Text == "debug");

			Assert.True(preprocessor.Defines[1]
									.Text == "macro");
		}

		[Fact]
		public void IfdefUndefTest()
		{
			// Arrange
			IList<SyntaxToken> tokens = ParseText(@"#define debug
#ifdef debug
#undef debug
#endif");

			var preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Defines.Count == 0);
		}

		[Fact]
		public void IfNDefDefineTest()
		{
			// Arrange
			IList<SyntaxToken> tokens = ParseText(@"#ifndef debug
#define debug
#endif");

			var preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Defines.Count == 1);

			Assert.True(preprocessor.Defines[0]
									.Text == "debug");
		}

		[Fact]
		public void IfNDefUndefTest()
		{
			// Arrange
			IList<SyntaxToken> tokens = ParseText(@"#define macro
#ifndef debug
#undef macro
#endif");

			var preprocessor = new Preprocessor(tokens);

			// Act
			preprocessor.Preprocess();

			// Assert
			Assert.True(preprocessor.Defines.Count == 0);
		}
	}
}
