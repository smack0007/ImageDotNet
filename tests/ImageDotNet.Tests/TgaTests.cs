using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ImageDotNet.Tests
{
    public partial class TgaTests
    {
        private readonly ITestOutputHelper _output;

        public TgaTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private static void AssertSaveAndLoad<T>(Image<T> expected)
            where T : unmanaged, IPixel
        {
            Image<T>? actual = null;

            using (var memory = new MemoryStream())
            {
                expected.SaveTga(memory);
                memory.Position = 0;
                actual = Image.LoadTga(memory).To<T>();
            }

            AssertEx.Equal(expected, actual);
        }
    }
}
