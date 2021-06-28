#region

using System;
using ChaoticOnyx.Hekate.Scaffolds;
using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Tests
{
    public class EmitingTests
    {
        [Fact]
        public void EmitTest()
        {
            // Arrange
            string expected = @"/*
	These are simple defaults for your project.
 */
#ifndef TEST
#endif
world
	fps = 25		// 25 frames per second
	icon_size = 32	// 32x32 icon size by default

	view = 6		// show up to 6 tiles outward from center (13x13 view)


// Make objects move 8 pixels per tick when walking


mob
	step_size = 8

obj
	step_size = 8

/proc/hello_world()
	world << ""Hello, world!""
	return

hello_world()
";

            TextToTokensScaffold scaffold1 = new(expected.AsMemory());
            TokensToTextScaffold scaffold2 = new(scaffold1.GetResult());

            // Act
            string result = scaffold2.GetResult()
                                     .ToString();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
