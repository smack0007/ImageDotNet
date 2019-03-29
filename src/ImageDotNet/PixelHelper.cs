using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    internal static partial class PixelHelper
    {
        public unsafe static T[] ToPixelArray<T>(this byte[] pixels)
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

        public static byte[] FlipVertically(this byte[] pixels, int width, int height, int bytesPerPixel)
        {
            Guard.NotNull(pixels, nameof(pixels));

            for (int y = 0; y < height / 2; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int top = ((y * width) + x) * bytesPerPixel;
                    int bottom = (((height - 1 - y) * width) + x) * bytesPerPixel;

                    for (int i = 0; i < bytesPerPixel; i++)
                    {
                        byte temp = pixels[top + i];
                        pixels[top + i] = pixels[bottom + i];
                        pixels[bottom + i] = temp;
                    }
                }
            }

            return pixels;
        }

        public static T[] FlipVertically<T>(this T[] pixels, int width, int height)
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

        public static unsafe U[] Convert<T, U>(this T[] source)
            where T : unmanaged, IPixel
            where U : unmanaged, IPixel
        {
            Guard.NotNull(source, nameof(source));

            var destination = new U[source.Length];

            fixed (void* sourcePtr = source)
            fixed (void* destinationPtr = destination)
            {
                if (typeof(T) == typeof(U))
                {
                    var lengthInBytes = source.Length * Marshal.SizeOf<T>();
                    Buffer.MemoryCopy(sourcePtr, destinationPtr, lengthInBytes, lengthInBytes);
                }
                else
                {
                    Convert(typeof(T), (byte*)sourcePtr, typeof(U), (byte*)destinationPtr, source.Length);
                }
            }

            return destination;
        }

        public static byte ConvertToGrayscale(byte r, byte g, byte b)
        {
            // Using lightness method documented here:
            // https://www.johndcook.com/blog/2009/08/24/algorithms-convert-color-grayscale/

            var min = Math.Min(r, Math.Min(g, b));
            var max = Math.Max(r, Math.Max(g, b));

            return (byte)((max + min) / 2);
        }

        private static unsafe void ConvertBgr24ToGray8(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Bgr24.SizeInBytes;
                int destinationOffset = i * Gray8.SizeInBytes;
                destinationPtr[destinationOffset] = ConvertToGrayscale(sourcePtr[sourceOffset + 2], sourcePtr[sourceOffset + 1], sourcePtr[sourceOffset]);
            }
        }

        private static unsafe void ConvertBgr24ToBgra32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Bgr24.SizeInBytes;
                int destinationOffset = i * Bgra32.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset + 1];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset + 2];
            }
        }

        private static unsafe void ConvertBgr24ToRgb24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int offset = i * Bgr24.SizeInBytes;
                destinationPtr[offset] = sourcePtr[offset + 2];
                destinationPtr[offset + 1] = sourcePtr[offset + 1];
                destinationPtr[offset + 2] = sourcePtr[offset];
            }
        }

        private static unsafe void ConvertBgr24ToRgba32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Bgr24.SizeInBytes;
                int destinationOffset = i * Rgba32.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset + 2];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset + 1];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset];
            }
        }

        private static unsafe void ConvertBgra32ToBgr24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Bgra32.SizeInBytes;
                int destinationOffset = i * Bgr24.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset + 1];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset + 2];
            }
        }

        private static unsafe void ConvertBgra32ToGray8(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Bgra32.SizeInBytes;
                int destinationOffset = i * Gray8.SizeInBytes;
                destinationPtr[destinationOffset] = ConvertToGrayscale(sourcePtr[sourceOffset + 2], sourcePtr[sourceOffset + 1], sourcePtr[sourceOffset]);
            }
        }

        private static unsafe void ConvertBgra32ToRgb24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Bgra32.SizeInBytes;
                int destinationOffset = i * Rgb24.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset + 2];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset + 1];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset];
            }
        }

        private static unsafe void ConvertBgra32ToRgba32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int offset = i * Bgra32.SizeInBytes;
                destinationPtr[offset] = sourcePtr[offset + 2];
                destinationPtr[offset + 1] = sourcePtr[offset + 1];
                destinationPtr[offset + 2] = sourcePtr[offset];
                destinationPtr[offset + 3] = sourcePtr[offset + 3];
            }
        }

        private static unsafe void ConvertGray8ToBgr24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Gray8.SizeInBytes;
                int destinationOffset = i * Bgr24.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset];
            }
        }

        private static unsafe void ConvertGray8ToBgra32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Gray8.SizeInBytes;
                int destinationOffset = i * Bgra32.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset];
            }
        }

        private static unsafe void ConvertGray8ToRgb24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Gray8.SizeInBytes;
                int destinationOffset = i * Rgb24.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset];
            }
        }

        private static unsafe void ConvertGray8ToRgba32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Gray8.SizeInBytes;
                int destinationOffset = i * Rgba32.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset];
            }
        }

        private static unsafe void ConvertRgb24ToBgr24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int offset = i * Bgr24.SizeInBytes;
                destinationPtr[offset] = sourcePtr[offset + 2];
                destinationPtr[offset + 1] = sourcePtr[offset + 1];
                destinationPtr[offset + 2] = sourcePtr[offset];
            }
        }

        private static unsafe void ConvertRgb24ToBgra32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Rgb24.SizeInBytes;
                int destinationOffset = i * Bgra32.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset + 2];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset + 1];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset];
            }
        }

        private static unsafe void ConvertRgb24ToGray8(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Rgb24.SizeInBytes;
                int destinationOffset = i * Gray8.SizeInBytes;
                destinationPtr[destinationOffset] = ConvertToGrayscale(sourcePtr[sourceOffset], sourcePtr[sourceOffset + 1], sourcePtr[sourceOffset + 2]);
            }
        }

        private static unsafe void ConvertRgb24ToRgba32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Rgb24.SizeInBytes;
                int destinationOffset = i * Rgba32.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset + 1];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset + 2];
            }
        }

        private static unsafe void ConvertRgba32ToBgr24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Rgba32.SizeInBytes;
                int destinationOffset = i * Bgr24.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset + 2];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset + 1];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset];
            }
        }

        private static unsafe void ConvertRgba32ToBgra32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int offset = i * Bgra32.SizeInBytes;
                destinationPtr[offset] = sourcePtr[offset + 2];
                destinationPtr[offset + 1] = sourcePtr[offset + 1];
                destinationPtr[offset + 2] = sourcePtr[offset];
                destinationPtr[offset + 3] = sourcePtr[offset + 3];
            }
        }

        private static unsafe void ConvertRgba32ToGray8(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Rgba32.SizeInBytes;
                int destinationOffset = i * Gray8.SizeInBytes;
                destinationPtr[destinationOffset] = ConvertToGrayscale(sourcePtr[sourceOffset], sourcePtr[sourceOffset + 1], sourcePtr[sourceOffset + 2]);
            }
        }

        private static unsafe void ConvertRgba32ToRgb24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int sourceOffset = i * Rgba32.SizeInBytes;
                int destinationOffset = i * Rgb24.SizeInBytes;
                destinationPtr[destinationOffset] = sourcePtr[sourceOffset];
                destinationPtr[destinationOffset + 1] = sourcePtr[sourceOffset + 1];
                destinationPtr[destinationOffset + 2] = sourcePtr[sourceOffset + 2];
            }
        }
    }
}
