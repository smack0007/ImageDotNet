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

        internal static byte[] FlipVertically(this byte[] pixels, int width, int height, int bytesPerPixel)
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

        internal static unsafe U[] Convert<T, U>(this T[] source)
            where T : unmanaged, IPixel
            where U : unmanaged, IPixel
        {
            Guard.NotNull(source, nameof(source));

            if (typeof(T) == typeof(U))
                return source as U[];

            var destination = new U[source.Length];

            fixed (void* sourcePtr = source)
            fixed (void* destinationPtr = destination)
            {
                Convert<T, U>((byte*)sourcePtr, (byte*)destinationPtr, source.Length);
            }

            return destination;
        }

        private static unsafe void ConvertBgr24ToBgra32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private static unsafe void ConvertBgra32ToBgr24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            throw new NotImplementedException();
        }

        private static unsafe void ConvertBgra32ToRgb24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private static unsafe void ConvertRgb24ToRgba32(byte* sourcePtr, byte* destinationPtr, int length)
        {
            throw new NotImplementedException();
        }

        private static unsafe void ConvertRgba32ToBgr24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            throw new NotImplementedException();
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

        private static unsafe void ConvertRgba32ToRgb24(byte* sourcePtr, byte* destinationPtr, int length)
        {
            throw new NotImplementedException();
        }
    }
}
