using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ImageDotNet.Tests
{
    public class TgaTests
    {
        private readonly ITestOutputHelper output;

        public TgaTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static readonly byte[] ImageData1x1 = new byte[]
        {
            1, 2, 3, 4
        };

        private static readonly byte[] ImageData2x2 = new byte[]
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16
        };

        private static readonly byte[] ImageData3x3 = new byte[]
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
            19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36
        };

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame1x1Image()
        {
            var image = new RgbaImage(1, 1, ImageData1x1);
            Image otherImage = null;

            using (var memory = new MemoryStream())
            {
                image.SaveTga(memory);
                memory.Position = 0;
                otherImage = Image.LoadTga(memory);
            }

            Assert.Equal(image, otherImage);
        }

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame2x2Image()
        {
            var image = new RgbaImage(2, 2, ImageData2x2);
            Image otherImage = null;

            using (var memory = new MemoryStream())
            {
                image.SaveTga(memory);
                memory.Position = 0;
                otherImage = Image.LoadTga(memory);
            }

            Assert.Equal(image, otherImage);
        }

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame3x3Image()
        {
            var image = new RgbaImage(3, 3, ImageData3x3);
            Image otherImage = null;

            using (var memory = new MemoryStream())
            {
                image.SaveTga(memory);
                memory.Position = 0;
                otherImage = Image.LoadTga(memory);
            }

            Assert.Equal(image, otherImage);
        }
    }
}
