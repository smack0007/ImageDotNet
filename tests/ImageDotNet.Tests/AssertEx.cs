using Xunit;

namespace ImageDotNet.Tests
{
    public static class AssertEx
    {
        public static void Equal<T>(Image<T> expected, Image<T> actual)
            where T: unmanaged, IPixel
        {
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.Height, actual.Height);

            for(int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }
    }
}
