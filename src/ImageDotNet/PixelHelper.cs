using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    public static class PixelHelper
    {
        public unsafe static byte[] ToByteArray<T>(T[] pixels)
            where T: unmanaged, IPixel
        {
            int pixelSize = Marshal.SizeOf<T>();

            int byteCount = pixels.Length * pixelSize;
            var buffer = new byte[byteCount];

            fixed (void* pixelsPtr = pixels)
            fixed (void* bufferPtr = buffer)
            {
                Buffer.MemoryCopy(pixelsPtr, bufferPtr, pixels.LongLength, pixels.LongLength);
            }

            return buffer;
        }

        public unsafe static T[] ToPixelArray<T>(byte[] pixels)
            where T: unmanaged, IPixel
        {
            var pixelSize = Marshal.SizeOf<T>();

            if (pixels.Length % pixelSize != 0)
                throw new ImageDotNetException($"Length of pixels array must be divisible by {pixelSize}.");

            var buffer = new T[pixels.Length / pixelSize];

            fixed (void* pixelsPtr = pixels)
            fixed (void* bufferPtr = buffer)
            {
                Buffer.MemoryCopy(pixelsPtr, bufferPtr, pixels.LongLength, pixels.LongLength);
            }

            return buffer;
        }
    }
}
