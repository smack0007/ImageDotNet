using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Bgr24 : IPixel, IEquatable<Bgr24>
    {
        public static readonly PixelFormat PixelFormat = PixelFormat.Bgr24;

        public byte B;

        public byte G;

        public byte R;

        public Bgr24(byte b, byte g, byte r)
        {
            B = b;
            G = g;
            R = r;
        }

        public override string ToString() => $"{B:000} {G:000} {R:000}";

        public void ReadFrom(byte[] buffer, int offset)
        {
            B = buffer[offset];
            G = buffer[offset + 1];
            R = buffer[offset + 2];
        }

        public bool Equals(Bgr24 other)
        {
            return B == other.B &&
                   G == other.G &&
                   R == other.R;
        }
    }
}
