using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using ImageDotNet.Tga;

namespace ImageDotNet
{
    public abstract partial class Image : IEnumerable<byte>
    {
        private readonly byte[] _pixels;

        public int Width { get; }

        public int Height { get; }

        public abstract int BytesPerPixel { get; }

        public int BitsPerPixel => this.BytesPerPixel * 8;

        public int Length => this._pixels.Length;

        public ref byte this[int index] => ref this._pixels[index];
        
        protected Image(int width, int height, byte[] pixels)
        {
            this.Width = width;
            this.Height = height;
            this._pixels = pixels;

            int pixelsLength = this.Width * this.Height * this.BytesPerPixel;
            if (this._pixels.Length != pixelsLength)
                throw new ImageDotNetException($"The format of the pixels is incorrect. The length of the pixels array should be {pixelsLength} but was {this._pixels.Length}");
        }

        public IEnumerator<byte> GetEnumerator() => (IEnumerator<byte>)this._pixels.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this._pixels.GetEnumerator();

        public ImageDataPointer GetDataPointer()
        {
            var handle = GCHandle.Alloc(this._pixels, GCHandleType.Pinned);
            return new ImageDataPointer(handle, this.Length);
        }
    }
}
