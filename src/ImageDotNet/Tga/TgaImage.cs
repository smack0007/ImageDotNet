using System.IO;

namespace ImageDotNet.Tga
{
    public static class TgaImage
    {
        public static Image Load(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            
            var header = br.ReadStruct<TgaHeader>();

            if (header.DataTypeCode != TgaDataType.UncompressedTrueColor && header.DataTypeCode != TgaDataType.RunLengthEncodedTrueColor)
                throw new ImageFormatException($"Only {nameof(TgaDataType.UncompressedTrueColor)} and {nameof(TgaDataType.RunLengthEncodedTrueColor)} TGA images are supported.");
            
            byte bytesPerPixel = (byte)(header.BitsPerPixel / 8);

            if (bytesPerPixel != 3 && bytesPerPixel != 4)
                throw new ImageFormatException("Only 24 and 32 bit TGA images are supported.");

            PixelFormat format = bytesPerPixel == 3 ? PixelFormat.RGB : PixelFormat.RGBA;

            byte[] pixels = null;
            int dataLength = header.Width * header.Height * bytesPerPixel;

            if (header.DataTypeCode == TgaDataType.UncompressedTrueColor)
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

            for (int y = 0; y < header.Height / 2; y++)
            {
                for (int x = 0; x < header.Width; x++)
                {
                    int top = ((y * header.Width) + x) * bytesPerPixel;
                    int bottom = (((header.Height - 1 - y) * header.Width) + x) * bytesPerPixel;

                    byte temp = pixels[top];
                    pixels[top] = pixels[top + 2];
                    pixels[top + 2] = temp;

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

            return new Image(header.Width, header.Height, format, pixels);
        }

        public static void Save(Image image, TgaDataType type, Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            byte[] header = new byte[18];
            header[2] = (byte)type;
            BinaryHelper.WriteLittleEndianUInt16(header, 12, (ushort)image.Width);
            BinaryHelper.WriteLittleEndianUInt16(header, 14, (ushort)image.Height);
            header[16] = (byte)(image.BytesPerPixel * 8);

            bw.Write(header);
        }
    }
}