using Xunit;

namespace ImageDotNet.Tests
{
    public partial class ImageTests
    {
        [Fact]
        public void ToAlwaysReturnsANewImage()
        {
            var image = new Image<Rgba32>(3, 3, TestData.Rgba32Images.Image3x3);
            var image2 = image.To<Rgba32>();

            Assert.NotSame(image, image2);
        }
    }
}
