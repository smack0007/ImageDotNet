using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    public sealed partial class Image<T> : Image, IEnumerable<T>
        where T : unmanaged, IPixel
    {
        private readonly T[] _pixels;

        public override Type PixelType => typeof(T);

        public override int Width { get; }

        public override int Height { get; }

        public override int BytesPerPixel { get; }

        public override int Length => _pixels.Length;

        public ref T this[int index] => ref _pixels[index];

        public Image(int width, int height, T[] pixels)
            : base()
        {
            Width = width;
            Height = height;
            BytesPerPixel = Marshal.SizeOf<T>();

            _pixels = pixels;

            if (this._pixels.Length != Width * Height)
                throw new ImageDotNetException($"The format of the pixels is incorrect. The length of the pixels array should be {Width * Height} but was {_pixels.Length}");
        }

        public override IEnumerator<IPixel> GetEnumerator() => _pixels.Cast<IPixel>().GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)_pixels).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _pixels.GetEnumerator();

        public ImageDataPointer GetDataPointer()
        {
            var handle = GCHandle.Alloc(this._pixels, GCHandleType.Pinned);
            return new ImageDataPointer(handle, this.Length);
        }
    }
}
