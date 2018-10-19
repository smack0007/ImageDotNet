using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ImageDotNet.Tests
{
    public class PngTests
    {
        private readonly ITestOutputHelper _output;

        public PngTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private static readonly Rgba32[] ImageData1x1 = new Rgba32[]
        {
            new Rgba32(1, 2, 3, 4),
        };

        private static readonly Rgba32[] ImageData2x2 = new Rgba32[]
        {
            new Rgba32(1, 2, 3, 4), new Rgba32(5, 6, 7, 8),
            new Rgba32(9, 10, 11, 12), new Rgba32(13, 14, 15, 16),
        };

        private static readonly Rgba32[] ImageData3x3 = new Rgba32[]
        {
            new Rgba32(1, 2, 3, 4), new Rgba32(5, 6, 7, 8), new Rgba32(9, 10, 11, 12),
            new Rgba32(13, 14, 15, 16), new Rgba32(17, 18, 19, 20), new Rgba32(21, 22, 23, 24),
            new Rgba32(25, 26, 27, 28), new Rgba32(29, 30, 31, 32), new Rgba32(33, 34, 35, 36),
        };

        private static void AssertSaveAndLoad(Image<Rgba32> expected)
        {
            Image<Rgba32> actual = null;

            using (var memory = new MemoryStream())
            {
                expected.SavePng(memory);
                memory.Position = 0;
                actual = (Image<Rgba32>)Image.LoadPng(memory);
            }

            AssertEx.Equal(expected, actual);
        }

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame1x1Image()
        {
            AssertSaveAndLoad(new Image<Rgba32>(1, 1, ImageData1x1));
        }

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame2x2Image()
        {
            AssertSaveAndLoad(new Image<Rgba32>(2, 2, ImageData2x2));
        }

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame3x3Image()
        {
            AssertSaveAndLoad(new Image<Rgba32>(3, 3, ImageData3x3));
        }
    }
}
