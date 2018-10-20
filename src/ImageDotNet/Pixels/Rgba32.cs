using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Rgba32 : IPixel, IEquatable<Rgba32>
    {
        public static readonly int SizeInBytes = Marshal.SizeOf<Rgba32>();

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

        public bool Equals(Rgba32 other)
        {
            return R == other.R &&
                   G == other.G &&
                   B == other.B &&
                   A == other.A;
        }
    }
}
