using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Bgra32 : IPixel, IEquatable<Bgra32>
    {
        public static readonly PixelFormat PixelFormat = PixelFormat.Bgra32;

        public byte B;

        public byte G;

        public byte R;

        public byte A;

        public Bgra32(byte b, byte g, byte r, byte a)
        {
            B = b;
            G = g;
            R = r;
            A = a;
        }

        public override string ToString() => $"{B:000} {G:000} {R:000} {A:000}";

        public void ReadFrom(byte[] buffer, int offset)
        {
            B = buffer[offset];
            G = buffer[offset + 1];
            R = buffer[offset + 2];
            A = buffer[offset + 3];
        }

        public bool Equals(Bgra32 other)
        {
            return B == other.B &&
                   G == other.G &&
                   R == other.R &&
                   A == other.A;
        }
    }
}
