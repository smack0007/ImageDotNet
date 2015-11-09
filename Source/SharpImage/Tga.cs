using System.IO;

namespace SharpImage
{
    public static class Tga
    {
        public static Image FromStream(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            
            byte[] header = br.ReadBytes(18);

            bool isCompressed = header[2] == 10;
            short width = (short)((header[13] << 8) | header[12]);
            short height = (short)((header[15] << 8) | header[14]);
            byte bpp = header[16];

            if (bpp != 24 && bpp != 32)
                throw new ImageFormatException("Only 24 and 32 bit TGA images are supported.");

            PixelFormat format = bpp == 24 ? PixelFormat.R8G8B8 : PixelFormat.R8G8B8A8;

            int dataLength = width * height * (bpp / 8);
            byte[] pixels = br.ReadBytes(dataLength);

            for (int y = 0; y < height / 2; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int top = ((y * width) + x) * (bpp / 8);
                    int bottom = (((height - 1 - y) * width) + x) * (bpp / 8);

                    byte temp = pixels[top];
                    pixels[top] = pixels[top + 2];
                    pixels[top + 2] = temp;

                    temp = pixels[bottom];
                    pixels[bottom] = pixels[bottom + 2];
                    pixels[bottom + 2] = temp;

                    for (int i = 0; i < bpp / 8; i++)
                    {
                        temp = pixels[top + i];
                        pixels[top + i] = pixels[bottom + i];
                        pixels[bottom + i] = temp;
                    }
                }
            }

            return new Image(width, height, format, pixels);
        }

    }
}
