using System;
using System.IO;

namespace SharpImage
{
    public class Image
    {
        private readonly byte[] pixels;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public PixelFormat Format { get; private set; }

        public int BytesPerPixel => this.Format.GetBytesPerPixel();

        public int Length => this.pixels.Length;

        public byte this[int index]
        {
            get { return this.pixels[index]; }
        }
        
        public Image(int width, int height, PixelFormat pixelFormat, byte[] pixels)
        {
            this.Width = width;
            this.Height = height;
            this.Format = pixelFormat;
            this.pixels = pixels;
        }

        public static ImageFormat GetImageFormatFromFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).Substring(1).ToLower();

            switch (fileExtension)
            {
                case "tga":
                    return ImageFormat.Tga;
            }

            throw new ImageFormatException($"ImageFormat cannot be determined for file {fileName}. Use overload which specifies the ImageFormat.");
        }

        public static Image FromFile(string fileName)
        {
            return FromFile(fileName, GetImageFormatFromFileName(fileName));
        }

        public static Image FromFile(string fileName, ImageFormat format)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return FromStream(stream, format);
            }
        }

        public static Image FromStream(Stream stream, ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Tga:
                    return Tga.FromStream(stream);
            }

            throw new NotImplementedException("ImageFormat." + format.ToString() + " not implemented.");
        }

        public void ChangePixelFormat(PixelFormat format)
        {
            if (format == this.Format)
                return;

            bool implemented = false;

            switch (this.Format)
            {
                case PixelFormat.RGB:
                    {
                        switch (format)
                        {
                            case PixelFormat.BGR:
                                this.SwapRedAndBluePixelValues();
                                implemented = true;
                                break;
                        }
                    }
                    break;

                case PixelFormat.RGBA:
                    {
                        switch (format)
                        {
                            case PixelFormat.BGRA:
                                this.SwapRedAndBluePixelValues();
                                implemented = true;
                                break;
                        }
                    }
                    break;

                case PixelFormat.BGR:
                    {
                        switch (format)
                        {
                            case PixelFormat.RGB:
                                this.SwapRedAndBluePixelValues();
                                implemented = true;
                                break;
                        }
                    }
                    break;

                case PixelFormat.BGRA:
                    {
                        switch (format)
                        {
                            case PixelFormat.RGBA:
                                this.SwapRedAndBluePixelValues();
                                implemented = true;
                                break;
                        }
                    }
                    break;
            }

            if (!implemented)
                throw new NotImplementedException($"Conversion from format {this.Format.ToString()} to {format.ToString()} is not implemented.");
        }

        private void SwapRedAndBluePixelValues()
        {
            for (int i = 0; i < this.pixels.Length; i += this.BytesPerPixel)
            {
                byte temp = this.pixels[i];
                this.pixels[i] = this.pixels[i + 2];
                this.pixels[i + 2] = temp;
            }
        }
    }
}
