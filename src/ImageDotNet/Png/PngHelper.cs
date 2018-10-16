using System;
using System.Collections.Generic;
using System.Text;

namespace ImageDotNet.Png
{
    internal static class PngHelper
    {
        public static readonly byte[] HeaderBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        public enum ChunkType
        {
            Other = -1,
            IHDR,
            PLTE,
            IDAT,
            IEND
        }
    }
}
