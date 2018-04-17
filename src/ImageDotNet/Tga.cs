using System.IO;

namespace ImageDotNet
{
    public static class Tga
    {
        public static Image FromStream(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            
            byte[] header = br.ReadBytes(18);

            byte type = header[2];

            if (type != (byte)TgaType.UncompressedTrueColor && type != (byte)TgaType.RunLengthEncodedTrueColor)
                throw new ImageFormatException("Only UncompressedTrueColor and RunLengthEncodedTrueColor TGA images are supported.");

            ushort width = BinaryHelper.ReadLittleEndianUInt16(header, 12);
            ushort height = BinaryHelper.ReadLittleEndianUInt16(header, 14);
            byte bpp = (byte)(header[16] / 8);

            if (bpp != 3 && bpp != 4)
                throw new ImageFormatException("Only 24 and 32 bit TGA images are supported.");

            PixelFormat format = bpp == 3 ? PixelFormat.BGR : PixelFormat.BGRA;

            byte[] pixels = null;
            int dataLength = width * height * bpp;

            if (type == (byte)TgaType.UncompressedTrueColor)
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
                        byte[] bytes = br.ReadBytes(bpp);

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
                        byte[] bytes = br.ReadBytes(nextByte * bpp);

                        for (int i = 0; i < bytes.Length; i++)
                        {
                            pixels[readCount] = bytes[i];
                            readCount++;
                        }
                    }
                }
            }

            for (int y = 0; y < height / 2; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int top = ((y * width) + x) * bpp;
                    int bottom = (((height - 1 - y) * width) + x) * bpp;

                    for (int i = 0; i < bpp; i++)
                    {
                        byte temp = pixels[top + i];
                        pixels[top + i] = pixels[bottom + i];
                        pixels[bottom + i] = temp;
                    }
                }
            }

            return new Image(width, height, format, pixels);
        }

        public static void Save(Image image, TgaType type, Stream stream)
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