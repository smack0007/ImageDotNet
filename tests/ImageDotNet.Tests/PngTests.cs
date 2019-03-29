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
    }
}
