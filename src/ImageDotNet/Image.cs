using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ImageDotNet.Tga;

namespace ImageDotNet
{
    public abstract partial class Image : IEnumerable<byte>
    {
        private readonly byte[] pixels;

        public int Width { get; }

        public int Height { get; }

        public abstract int BytesPerPixel { get; }

        public int BitsPerPixel => this.BytesPerPixel * 8;

        public int Length => this.pixels.Length;

        public ref byte this[int index] => ref this.pixels[index];
        
        protected Image(int width, int height, byte[] pixels)
        {
            this.Width = width;
            this.Height = height;
            this.pixels = pixels;

            int pixelsLength = this.Width * this.Height * this.BytesPerPixel;
            if (this.pixels.Length != pixelsLength)
                throw new ImageDotNetException($"The format of the pixels is incorrect. The length of the pixels array should be {pixelsLength} but was {this.pixels.Length}");
        }

        public IEnumerator<byte> GetEnumerator() => (IEnumerator<byte>)this.pixels.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.pixels.GetEnumerator();
    }
}
