using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rgb24 : IPixel, IEquatable<Rgb24>
    {
        public static readonly PixelFormat PixelFormat = PixelFormat.Rgb24;

        public byte R;

        public byte G;

        public byte B;

        public Rgb24(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public override string ToString() => $"{R:000} {G:000} {B:000}";

        public void ReadFrom(byte[] buffer, int offset)
        {
            R = buffer[offset];
            G = buffer[offset + 1];
            B = buffer[offset + 2];
        }

        public bool Equals(Rgb24 other)
        {
            return R == other.R &&
                   G == other.G &&
                   B == other.B;
        }
    }
}
