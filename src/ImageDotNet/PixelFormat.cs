namespace ImageDotNet
{
    public enum PixelFormat
    {
        RGB,

        RGBA,

        BGR,

        BGRA
    }

    public static class PixelFormatExtensions
    {
        public static int GetBytesPerPixel(this PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.RGB:
                case PixelFormat.BGR:
                    return 3;

                case PixelFormat.RGBA:
                case PixelFormat.BGRA:
                    return 4;
            }

            return 0;
        }
    }
}
