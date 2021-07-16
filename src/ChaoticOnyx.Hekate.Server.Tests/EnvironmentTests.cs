using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChaoticOnyx.Hekate.Server.Tests
{
    public class EnvironmentTests : IDisposable
    {
        [Fact]
        public async Task TestParsingAsync()
        {
            // Arrange
            var files = TestUtils.CreateTestEnvironment();
            var (_, _, environmentService) = TestUtils.ProvideServices();

            // Act
            await environmentService.ParseEnvironmentAsync(files.First(f => f.Extension == ".dme"));
            var codeFiles = environmentService.Files;

            // Assert
            Assert.NotNull(codeFiles);
            Assert.Equal(4, codeFiles.Count);
            Assert.True(codeFiles.TrueForAll(c => c.Tokens.Count > 0));
        }

        public void Dispose()
        {
            TestUtils.DeleteTestEnvironment();
        }
    }
}
