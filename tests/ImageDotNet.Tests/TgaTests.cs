using System;
using System.IO;
using Xunit;
using ImageDotNet;
using ImageDotNet.Tga;
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

        private static readonly byte[] ImageData = new byte[]
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16
        };

        [Fact]
        public void AfterSaveAndLoadPixelsAreTheSame()
        {
            var image = new RgbaImage(2, 2, ImageData);
            Image otherImage = null;

            using (var memory = new MemoryStream())
            {
                image.SaveTga(memory);
                memory.Position = 0;
                otherImage = Image.LoadTga(memory);
            }

            Assert.Equal(image.Length, otherImage.Length);
            for (int i = 0; i < image.Length; i++)
            {
                Assert.Equal(image[i], otherImage[i]);
            }
        }
    }
}
