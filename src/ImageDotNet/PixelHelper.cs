using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    public static partial class PixelHelper
    {
        internal unsafe static byte[] ToByteArray<T>(this T[] pixels)
            where T: unmanaged, IPixel
        {
            int pixelSize = Marshal.SizeOf<T>();

            int byteCount = pixels.Length * pixelSize;
            var buffer = new byte[byteCount];

            fixed (void* pixelsPtr = pixels)
            fixed (void* bufferPtr = buffer)
            {
                Buffer.MemoryCopy(pixelsPtr, bufferPtr, buffer.LongLength, buffer.LongLength);
            }

            return buffer;
        }

        internal unsafe static T[] ToPixelArray<T>(this byte[] pixels)
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

        internal unsafe static T[] Clone<T>(T[] pixels)
            where T : unmanaged, IPixel
        {
            Guard.NotNull(pixels, nameof(pixels));

            var pixelSizeInBytes = Marshal.SizeOf<T>();

            var buffer = new T[pixels.Length];

            var byteCount = pixels.LongLength * pixelSizeInBytes;

            fixed (void* pixelsPtr = pixels)
            fixed (void* bufferPtr = buffer)
            {
                Buffer.MemoryCopy(pixelsPtr, bufferPtr, byteCount, byteCount);
            }

            return buffer;
        }

        internal static U[] Convert<T, U>(this T[] pixels)
            where T: struct, IPixel
            where U: struct, IPixel
        {
            Guard.NotNull(pixels, nameof(pixels));

            if (typeof(T) == typeof(U))
                return pixels as U[];

            var newPixels = new U[pixels.Length];
            
            for (int i = 0; i < newPixels.Length; i++)
            {

            }

            return newPixels;
        }

        internal static T[] FlipVertically<T>(this T[] pixels, int width, int height)
            where T: struct, IPixel
        {
            Guard.NotNull(pixels, nameof(pixels));

            for (int y = 0; y < height / 2; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int top = (y * width) + x;
                    int bottom = ((height - 1 - y) * width) + x;

                    T temp = pixels[top];
                    pixels[top] = pixels[bottom];
                    pixels[bottom] = temp;
                }
            }

            return pixels;
        }
    }
}
