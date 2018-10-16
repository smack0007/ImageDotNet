using System;
using System.Collections;
using System.Collections.Generic;

namespace ImageDotNet
{
    public abstract partial class Image : IEnumerable<IPixel>
    {
        public abstract Type PixelType { get; }

        public abstract int Width { get; }

        public abstract int Height { get; }

        public abstract int BytesPerPixel { get; }

        public int BitsPerPixel => this.BytesPerPixel * 8;

        public abstract int Length { get; }

        public abstract IEnumerator<IPixel> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
