using Xunit;

namespace ImageDotNet.Tests
{
    public partial class ImageTests
    {
        [Fact]
        public void ForEachPixelCanModifyPixels()
        {
            var image = new Image<Rgb24>(3, 3, TestData.Rgb24Images.Image3x3).ToRgba32();

            image.ForEachPixel((ref Rgba32 x) => x.A = 255);

            for (int i = 0; i < image.Length; i++)
                Assert.Equal(255, image[i].A);
        }

        [Fact]
        public void ToAlwaysReturnsANewImage()
        {
            var image = new Image<Rgba32>(3, 3, TestData.Rgba32Images.Image3x3);
            var image2 = image.ToRgba32();

            Assert.NotSame(image, image2);
        }
    }
}
