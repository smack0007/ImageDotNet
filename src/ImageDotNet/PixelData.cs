using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    internal unsafe struct PixelData : IDisposable
    {
        private GCHandle _handle;

        private byte* _pointer;

        public Type PixelType { get; private set; }

        public int LengthInPixels { get; private set; }

        public int BytesPerPixel => Marshal.SizeOf(PixelType);

        public int Length => LengthInPixels * BytesPerPixel;

        public byte this[int offset] => _pointer[offset];

        private PixelData(GCHandle handle, Type pixelType, int length)
        {
            _handle = handle;
            _pointer = (byte*)_handle.AddrOfPinnedObject().ToPointer();

            PixelType = pixelType;
            LengthInPixels = length;
        }

        public void Dispose()
        {
            _handle.Free();
        }

        private void SetData<T>(T[] pixels)
        {
            _handle.Free();

            var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);

            _handle = handle;
            _pointer = (byte*)_handle.AddrOfPinnedObject().ToPointer();

            PixelType = typeof(T);
            LengthInPixels = pixels.Length;
        }

        public static PixelData Create<T>(T[] pixels)
            where T : unmanaged, IPixel
        {
            var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            return new PixelData(handle, typeof(T), pixels.Length);
        }

        public static PixelData Clone<T>(T[] pixels)
            where T : unmanaged, IPixel
        {
            var buffer = new T[pixels.Length];

            var pixelSizeInBytes = Marshal.SizeOf<T>();
            var byteCount = pixels.LongLength * pixelSizeInBytes;

            fixed (void* pixelsPtr = pixels)
            fixed (void* bufferPtr = buffer)
            {
                Buffer.MemoryCopy(pixelsPtr, bufferPtr, byteCount, byteCount);
            }

            return Create(buffer);
        }

        public void Convert<T>()
            where T : unmanaged, IPixel
        {
            if (typeof(T) == PixelType)
                return;

            var buffer = new T[LengthInPixels];

            fixed (void* bufferPtr = buffer)
            {
                PixelHelper.Convert(PixelType, _pointer, typeof(T), (byte*)bufferPtr, LengthInPixels);
            }

            SetData(buffer);
        }

        public void FlipVertically(int width, int height)
        {
            for (int y = 0; y < height / 2; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int top = ((y * width) + x) * BytesPerPixel;
                    int bottom = (((height - 1 - y) * width) + x) * BytesPerPixel;

                    for (int i = 0; i < BytesPerPixel; i++)
                    {
                        byte temp = _pointer[top + i];
                        _pointer[top + i] = _pointer[bottom + i];
                        _pointer[bottom + i] = temp;
                    }
                }
            }
        }
    }
}
