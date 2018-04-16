using System.IO;

namespace SharpImage
{
    public static class Tga
    {
        const byte UncompressedTrueColor = 2;
        const byte RunLengthEncodedTrueColor = 10;

        public static Image FromStream(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            
            byte[] header = br.ReadBytes(18);

            byte type = header[2];

            if (type != UncompressedTrueColor && type != RunLengthEncodedTrueColor)
                throw new ImageFormatException("Only UncompressedTrueColor and RunLengthEncodedTrueColor TGA images are supported.");

            short width = (short)((header[13] << 8) | header[12]);
            short height = (short)((header[15] << 8) | header[14]);
            byte bpp = (byte)(header[16] / 8);

            if (bpp != 3 && bpp != 4)
                throw new ImageFormatException("Only 24 and 32 bit TGA images are supported.");

            PixelFormat format = bpp == 3 ? PixelFormat.BGR : PixelFormat.BGRA;

            byte[] pixels = null;
            int dataLength = width * height * bpp;

            if (type == UncompressedTrueColor)
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

    }
}
