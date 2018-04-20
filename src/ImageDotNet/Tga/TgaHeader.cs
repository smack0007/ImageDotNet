using System;
using System.Runtime.InteropServices;

namespace ImageDotNet.Tga
{
    public static class TgaHeader
    {
        public const int SizeInBytes = 18;

        public const int DataTypeCode = 2;

        public const int Width = 12;

        public const int Height = 14;

        public const int BitsPerPixel = 16;
    }
}