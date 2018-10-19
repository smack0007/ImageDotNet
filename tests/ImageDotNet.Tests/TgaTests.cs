using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ImageDotNet.Tests
{
    public class TgaTests
    {
        private readonly ITestOutputHelper _output;

        public TgaTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private static readonly Bgra32[] ImageData1x1 = new Bgra32[]
        {
            new Bgra32(1, 2, 3, 4),
        };

        private static readonly Bgra32[] ImageData2x2 = new Bgra32[]
        {
            new Bgra32(1, 2, 3, 4), new Bgra32(5, 6, 7, 8),
            new Bgra32(9, 10, 11, 12), new Bgra32(13, 14, 15, 16),
        };

        private static readonly Bgra32[] ImageData3x3 = new Bgra32[]
        {
            new Bgra32(1, 2, 3, 4), new Bgra32(5, 6, 7, 8), new Bgra32(9, 10, 11, 12),
            new Bgra32(13, 14, 15, 16), new Bgra32(17, 18, 19, 20), new Bgra32(21, 22, 23, 24),
            new Bgra32(25, 26, 27, 28), new Bgra32(29, 30, 31, 32), new Bgra32(33, 34, 35, 36),
        };

        private static void AssertSaveAndLoad(Image<Bgra32> expected)
        {
            Image<Bgra32> actual = null;

            using (var memory = new MemoryStream())
            {
                expected.SaveTga(memory);
                memory.Position = 0;
                actual = (Image<Bgra32>)Image.LoadTga(memory);
            }

            AssertEx.Equal(expected, actual);
        }

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame1x1Image()
        {
            AssertSaveAndLoad(new Image<Bgra32>(1, 1, ImageData1x1));
        }

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame2x2Image()
        {
            AssertSaveAndLoad(new Image<Bgra32>(2, 2, ImageData2x2));
        }

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame3x3Image()
        {
            AssertSaveAndLoad(new Image<Bgra32>(3, 3, ImageData3x3));
        }
    }
}
