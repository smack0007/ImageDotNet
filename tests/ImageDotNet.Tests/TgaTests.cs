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
    }
}
