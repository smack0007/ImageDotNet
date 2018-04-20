using System;
using System.IO;
using ImageDotNet.Tga;

namespace ImageDotNet
{
    public abstract partial class Image
    {
        private readonly byte[] pixels;

        public int Width { get; }

        public int Height { get; }

        public abstract int BytesPerPixel { get; }

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

        public static ImageFileFormat GetImageFileFormatFromFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).Substring(1).ToLower();

            switch (fileExtension)
            {
                case "tga":
                    return ImageFileFormat.Tga;
            }

            throw new ImageDotNetException($"{nameof(ImageFileFormat)} cannot be determined for file '{fileName}'. Use overload which specifies the ImageFormat.");
        }
    }
}
