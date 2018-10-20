using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Bgr24 : IPixel, IEquatable<Bgr24>
    {
        public static readonly int SizeInBytes = Marshal.SizeOf<Bgr24>();

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

        public bool Equals(Bgr24 other)
        {
            return B == other.B &&
                   G == other.G &&
                   R == other.R;
        }
    }
}
