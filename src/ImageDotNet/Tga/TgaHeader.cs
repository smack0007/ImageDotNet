using System;
using System.Runtime.InteropServices;

namespace ImageDotNet.Tga
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TgaHeader
    {
        public byte IdLength;

        public byte ColorMapType;

        public TgaDataType DataTypeCode;

        public ushort ColorMapOrigin;

        public ushort ColorMapLength;

        public byte ColorMapDepth;

        public ushort XOrigin;

        public ushort YOrigin;

        public ushort Width;

        public ushort Height;

        public byte BitsPerPixel;

        public byte ImageDescriptor;
    }
}