using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    public sealed partial class Image<T> : IImage
        where T : unmanaged, IPixel
    {
        private T[] _pixels;

        public Type PixelType => typeof(T);

        public int Width { get; }

        public int Height { get; }

        public int BytesPerPixel => Marshal.SizeOf<T>();

        public int Length => _pixels.Length;

        public ref T this[int index] => ref _pixels[index];

        public ref T this[int x, int y] => ref _pixels[(y * Width) + x];

        public Image(int width, int height, T[] pixels)
            : base()
        {
            Width = width;
            Height = height;

            _pixels = pixels;

            if (this._pixels.Length != Width * Height)
                throw new ImageDotNetException($"The format of the pixels is incorrect. The length of the pixels array should be {Width * Height} but was {_pixels.Length}");
        }
        
        public ImageDataPointer GetDataPointer()
        {
            var handle = GCHandle.Alloc(_pixels, GCHandleType.Pinned);
            return new ImageDataPointer(handle, Length);
        }

        public void ForEachPixel(Action<T> action)
        {
            Guard.NotNull(action, nameof(action));

            foreach (var pixel in _pixels)
                action(pixel);
        }

        void IImage.ForEachPixel(Action<IPixel> action)
        {
            Guard.NotNull(action, nameof(action));

            foreach (var pixel in _pixels)
                action(pixel);
        }

        public Image<U> To<U>()
            where U : unmanaged, IPixel
        {
            return new Image<U>(Width, Height, PixelHelper.Convert<T, U>(_pixels));
        }

        public void FlipVertically()
        {
            _pixels = PixelHelper.FlipVertically(_pixels, Width, Height);
        }
    }
}
