using System.Runtime.InteropServices;

namespace ImageDotNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rgb24 : IPixel
    {
        public byte R;

        public byte G;

        public byte B;

        public override string ToString() => $"{R:000} {G:000} {B:000}";

        public void ReadFrom(byte[] buffer, int offset)
        {
            R = buffer[offset];
            G = buffer[offset + 1];
            B = buffer[offset + 2];
        }
    }
}
