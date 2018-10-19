using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rgba32 : IPixel, IEquatable<Rgba32>
    {
        public static readonly PixelFormat PixelFormat = PixelFormat.Rgba32;

        public byte R;

        public byte G;

        public byte B;

        public byte A;

        public Rgba32(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public override string ToString() => $"{R:000} {G:000} {B:000} {A:000}";

        public void ReadFrom(byte[] buffer, int offset)
        {
            R = buffer[offset];
            G = buffer[offset + 1];
            B = buffer[offset + 2];
            A = buffer[offset + 3];
        }

        public bool Equals(Rgba32 other)
        {
            return R == other.R &&
                   G == other.G &&
                   B == other.B &&
                   A == other.A;
        }
    }
}
