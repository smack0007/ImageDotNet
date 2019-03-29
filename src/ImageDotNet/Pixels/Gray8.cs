using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Gray8 : IPixel, IEquatable<Gray8>
    {
        public static readonly int SizeInBytes = Marshal.SizeOf<Gray8>();

        public byte L;

        public Gray8(byte l)
        {
            L = l;
        }

        public override string ToString() => $"{L:000}";

        public bool Equals(Gray8 other)
        {
            return L == other.L;
        }
    }
}
