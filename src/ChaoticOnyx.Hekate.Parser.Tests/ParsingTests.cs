using System.Linq;
using ChaoticOnyx.Hekate.Parser.SyntaxNodes;
using Xunit;

namespace ChaoticOnyx.Hekate.Parser.Tests
{
	public class ParsingTests
	{
		private static CompilationUnitNode ParseText(string text)
		{
			var unit = new CompilationUnit(text);
			unit.Parse();

			return unit.Parser.Root;
		}
		
		[Theory]
		[InlineData("/datum/mytype/type")]
		[InlineData("datum/mytype/type")]
		[InlineData(@"datum
	mytype/type")]
		[InlineData(@"datum
	mytype
		type")]
		public void TypeDeclarationTest(string text)
		{
			// Arrange
			// Act
			CompilationUnitNode             root = ParseText(text);
			DeclarationNode type = root.Declarations[0];
			
			// Assert
			Assert.Equal(1, root.Declarations.Count);
			Assert.True(type.Kind == NodeKind.TypeDeclaration);
			Assert.True(type is TypeDeclarationNode { Name: "type" });
			Assert.Contains(type.FullPath, t => t.Text == "datum");
			Assert.Contains(type.FullPath, t => t.Text == "mytype");
		}

		[Theory]
		[InlineData("/var/a")]
		[InlineData("var/a")]
		[InlineData(@"var
	a")]
		[InlineData("/datum/mytype/type/var/a", 2)]
		[InlineData("datum/mytype/type/var/a", 2)]
		[InlineData(@"datum
	mytype/type/var/a", 2)]
		[InlineData(@"datum
	mytype
		type/var/a", 2)]
		[InlineData(@"datum
	mytype
		type
			var/a", 2)]
		[InlineData(@"datum
	mytype
		type
			var
				a", 2)]
		public void VariableDeclarationTest(string text, int expectedDeclarations = 1)
		{
			// Arrange
			// Act
			CompilationUnitNode root = ParseText(text);
			DeclarationNode     variable = root.Declarations.First(d => d.Kind == NodeKind.VariableDeclaration);
			
			// Assert
			Assert.Equal(expectedDeclarations, root.Declarations.Count);
			Assert.True(variable.Kind == NodeKind.VariableDeclaration);
			Assert.True(variable is VariableDeclarationNode { Name: "a" });
		}
	}
}
