using System;
using System.Runtime.InteropServices;
using ImageDotNet.PixelFormats;

namespace ImageDotNet
{
    public sealed partial class Image<T> : IImage
        where T : unmanaged, IPixel
    {
        private readonly T[] _pixels;

        public Type PixelType => typeof(T);

        public int Width { get; }

        public int Height { get; }

        public int BytesPerPixel => Marshal.SizeOf<T>();

        public int Length => _pixels.Length;

        public ref T this[int index] => ref _pixels[index];

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
            var handle = GCHandle.Alloc(this._pixels, GCHandleType.Pinned);
            return new ImageDataPointer(handle, this.Length);
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

        public Image<Rgb24> ToRgb24()
        {
            if (typeof(T) == typeof(Rgb24))
                return this as Image<Rgb24>;

            throw new NotImplementedException($"{nameof(Rgb24)} conversion not yet implemented.");
        }

        public Image<Rgba32> ToRgba32()
        {
            if (typeof(T) == typeof(Rgba32))
                return this as Image<Rgba32>;

            throw new NotImplementedException($"{nameof(Rgba32)} conversion not yet implemented.");
        }
    }
}
