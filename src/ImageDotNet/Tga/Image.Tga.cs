using System.IO;
using ImageDotNet.Tga;

namespace ImageDotNet
{
    public abstract partial class Image
    {
        public static Image LoadTga(string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return LoadTga(file);
            }
        }

        public static Image LoadTga(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            
            var header = br.ReadBytes(TgaHeader.SizeInBytes);

            var dataType = (TgaDataType)header[TgaHeader.DataTypeCode];

            if (dataType != TgaDataType.UncompressedTrueColor && dataType != TgaDataType.RunLengthEncodedTrueColor)
                throw new ImageDotNetException($"Only {nameof(TgaDataType.UncompressedTrueColor)} and {nameof(TgaDataType.RunLengthEncodedTrueColor)} TGA images are supported.");

            ushort width = BinaryHelper.ReadLittleEndianUInt16(header, TgaHeader.Width);
            ushort height = BinaryHelper.ReadLittleEndianUInt16(header, TgaHeader.Height);
            byte bytesPerPixel = (byte)(header[TgaHeader.BitsPerPixel] / 8);

            if (bytesPerPixel != 3 && bytesPerPixel != 4)
                throw new ImageDotNetException("Only 24 and 32 bit TGA images are supported.");

            byte[] pixels = null;
            int dataLength = width * height * bytesPerPixel;

            if (dataType == TgaDataType.UncompressedTrueColor)
            {
                pixels = br.ReadBytes(dataLength);
            }
            else
            {
                pixels = new byte[dataLength];

                int readCount = 0;
                while (readCount < dataLength)
                {
                    byte nextByte = br.ReadByte();

                    if ((nextByte & 0x80) != 0) // Check high bit
                    {
                        nextByte -= 127; 
                        byte[] bytes = br.ReadBytes(bytesPerPixel);

                        for (int i = 0; i < nextByte; i++)
                        {
                            for (int j = 0; j < bytes.Length; j++)
                            {
                                pixels[readCount] = bytes[j];
                                readCount++;
                            }
                        }
                    }
                    else // Raw chunk
                    {
                        nextByte += 1;
                        byte[] bytes = br.ReadBytes(nextByte * bytesPerPixel);

                        for (int i = 0; i < bytes.Length; i++)
                        {
                            pixels[readCount] = bytes[i];
                            readCount++;
                        }
                    }
                }
            }

            // Half height because we have to flip the image.
            for (int y = 0; y < height / 2; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int top = ((y * width) + x) * bytesPerPixel;
                    int bottom = (((height - 1 - y) * width) + x) * bytesPerPixel;

                    // Change BGR => RGB 
                    byte temp = pixels[top];
                    pixels[top] = pixels[top + 2];
                    pixels[top + 2] = temp;

                    // Change BGR => RGB 
                    temp = pixels[bottom];
                    pixels[bottom] = pixels[bottom + 2];
                    pixels[bottom + 2] = temp;

                    for (int i = 0; i < bytesPerPixel; i++)
                    {
                        temp = pixels[top + i];
                        pixels[top + i] = pixels[bottom + i];
                        pixels[bottom + i] = temp;
                    }
                }
            }

            // If we have an odd number of lines, we need to flip BGR => RGB on
            // the middle line.
            if (height % 2 == 1)
            {
                for (int x = 0; x < width; x++)
                {
                    int middle = (((height / 2) * width) + x) * bytesPerPixel;

                    byte temp = pixels[middle];
                    pixels[middle] = pixels[middle + 2];
                    pixels[middle + 2] = temp;
                }
            }

            if (bytesPerPixel == 3)
            {
                return new RgbImage(width, height, pixels);
            }
            else
            {
                return new RgbaImage(width, height, pixels);
            }
        }

        public void SaveTga(string fileName, TgaDataType dataType = TgaDataType.UncompressedTrueColor)
        {
            using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                this.SaveTga(file, dataType);
            }
        } 

        public void SaveTga(Stream stream, TgaDataType dataType = TgaDataType.UncompressedTrueColor)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            var header = new byte[TgaHeader.SizeInBytes];
            header[TgaHeader.DataTypeCode] = (byte)dataType;
            BinaryHelper.WriteLittleEndianUInt16(header, TgaHeader.Width, (ushort)this.Width);
            BinaryHelper.WriteLittleEndianUInt16(header, TgaHeader.Height, (ushort)this.Height);
            header[TgaHeader.BitsPerPixel] = (byte)(this.BytesPerPixel * 8);

            bw.Write(header);

            byte[] pixels = new byte[this.Width * this.Height * this.BytesPerPixel];

            for (int y = 0; y < this.Height / 2; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    int top = ((y * this.Width) + x) * this.BytesPerPixel;
                    int bottom = (((this.Height - 1 - y) * this.Width) + x) * this.BytesPerPixel;

                    if (this.BytesPerPixel >= 3)
                    {
                        // We have to flip RGB to BGR and flip bottom and top
                        pixels[top] = this[bottom + 2];
                        pixels[top + 1] = this[bottom + 1];
                        pixels[top + 2] = this[bottom];

                        pixels[bottom] = this[top + 2];
                        pixels[bottom + 1] = this[top + 1];
                        pixels[bottom + 2] = this[top];

                        if (this.BytesPerPixel == 4)
                        {
                            pixels[top + 3] = this[bottom + 3];
                            pixels[bottom + 3] = this[top + 3];
                        }
                    }
                }
            }

            // If we have an odd number of lines, we need to copy the middle line.
            if (this.Height % 2 == 1)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    int middle = (((this.Height / 2) * this.Width) + x) * this.BytesPerPixel;

                    if (this.BytesPerPixel >= 3)
                    {
                        pixels[middle] = this[middle + 2];
                        pixels[middle + 1] = this[middle + 1];
                        pixels[middle + 2] = this[middle];

                        if (this.BytesPerPixel == 4)
                            pixels[middle + 3] = this[middle + 3];
                    }
                }
            }

            bw.Write(pixels);

            bw.Flush();
        }
    }
}