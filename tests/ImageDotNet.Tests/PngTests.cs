using System.IO;
using Xunit.Abstractions;

namespace ImageDotNet.Tests
{
    public partial class PngTests
    {
        private readonly ITestOutputHelper _output;

        public PngTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private static void AssertSaveAndLoad<T>(Image<T> expected)
            where T : unmanaged, IPixel
        {
            Image<T>? actual = null;

            using (var memory = new MemoryStream())
            {
                expected.SavePng(memory);
                memory.Position = 0;
                actual = Image.LoadPng(memory).To<T>();
            }

            AssertEx.Equal(expected, actual);
        }
    }
}
