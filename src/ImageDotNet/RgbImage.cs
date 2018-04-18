namespace ImageDotNet
{
    public class RgbImage : Image
    {
        public override int BytesPerPixel => 3; 

        public RgbImage(int width, int height, byte[] pixels)
            : base(width, height, pixels)
        {
        }
    }
}