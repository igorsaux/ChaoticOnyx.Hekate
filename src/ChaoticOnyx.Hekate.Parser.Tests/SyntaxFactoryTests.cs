#region

using ChaoticOnyx.Hekate.Parser.ChaoticOnyx.Tools.StyleCop;
using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Parser.Tests
{
    public class SyntaxFactoryTests
    {
        private readonly SyntaxFactory _factory = SyntaxFactory.CreateFactory(CodeStyle.Default);

        [Fact]
        public void SingleLineCommentTest()
        {
            // Arrange
            var expected = "// This is a comment\n";
            var token    = _factory.SingleLineComment(" This is a comment");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MultiLineCommentTest()
        {
            // Arrange
            var expected = "/*\n  Hello!\n*/";
            var token    = _factory.MultiLineComment("\n  Hello!\n");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IdentifierTest()
        {
            // Arrange
            var expected = "var";
            var token    = _factory.Identifier("var");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TextLiteralTest()
        {
            // Arrange
            var expected = "\"Test\"";
            var token    = _factory.TextLiteral("Test");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NumericalLiteralTest()
        {
            // Arrange
            var expected = "12 7.8 12.9";

            SyntaxToken[] tokens =
            {
                _factory.NumericalLiteral(12)
                        .WithTrails(_factory.WhiteSpace(" ")),
                _factory.NumericalLiteral(7.8)
                        .WithTrails(_factory.WhiteSpace(" ")),
                _factory.NumericalLiteral((float) 12.9)
            };

            // Act
            var unit   = new CompilationUnit(tokens);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PathLiteralTest()
        {
            // Arrange
            var expected = "'sound/mysound.ogg'";
            var token    = _factory.PathLiteral("sound/mysound.ogg");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ForKeywordTest()
        {
            // Arrange
            var expected = "for";
            var token    = _factory.ForKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NewKeywordTest()
        {
            // Arrange
            var expected = "new";
            var token    = _factory.NewKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GlobalKeywordTest()
        {
            // Arrange
            var expected = "global";
            var token    = _factory.GlobalKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowKeywordTest()
        {
            // Arrange
            var expected = "throw";
            var token    = _factory.ThrowKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CatchKeywordTest()
        {
            // Arrange
            var expected = "catch";
            var token    = _factory.CatchKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TryKeywordTest()
        {
            // Arrange
            var expected = "try";
            var token    = _factory.TryKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VarKeywordTest()
        {
            // Arrange
            var expected = "var";
            var token    = _factory.VarKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerbKeywordTest()
        {
            // Arrange
            var expected = "verb";
            var token    = _factory.VerbKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ProcKeywordTest()
        {
            // Arrange
            var expected = "proc";
            var token    = _factory.ProcKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InKeywordTest()
        {
            // Arrange
            var expected = "in";
            var token    = _factory.InKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfKeywordTest()
        {
            // Arrange
            var expected = "if";
            var token    = _factory.IfKeyword();

            // Act
            var unit  = new CompilationUnit(token);
            var resul = unit.Emit();

            // Assert
            Assert.Equal(expected, resul);
        }

        [Fact]
        public void ElseKeywordTest()
        {
            // Arrange
            var expected = "else";
            var token    = _factory.ElseKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetKeywordTest()
        {
            // Arrange
            var expected = "set";
            var token    = _factory.SetKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsKeywordTest()
        {
            // Arrange
            var expected = "as";
            var token    = _factory.AsKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhileKeywordTest()
        {
            // Arrange
            var expected = "while";
            var token    = _factory.WhileKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DefineDirectiveTest()
        {
            // Arrange
            var expected = "#define\n";
            var token    = _factory.DefineDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IncludeDirectiveTest()
        {
            // Arrange
            var expected = "#include\n";
            var token    = _factory.IncludeDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfDefDirective()
        {
            // Arrange
            var expected = "#ifdef\n";
            var token    = _factory.IfDefDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfNDefDirective()
        {
            // Arrange
            var expected = "#ifndef\n";
            var token    = _factory.IfNDefDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EndIfDirective()
        {
            // Arrange
            var expected = "#endif\n";
            var token    = _factory.EndIfDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
