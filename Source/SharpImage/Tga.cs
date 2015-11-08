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

            int dataLength = width * height * bpp;
            byte[] pixels = br.ReadBytes(dataLength);

            for (int i = 0; i < pixels.Length; i += bpp)
            {
                byte temp = pixels[i];
                pixels[i] = pixels[i + 2];
                pixels[i + 2] = temp;
            }

            return new Image(width, height, PixelFormat.R8G8B8A8, pixels);
        }

    }
}
