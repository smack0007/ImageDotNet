namespace ImageDotNet
{
    public class RgbaImage : Image
    {
        public override int BytesPerPixel => 4; 

        public RgbaImage(int width, int height, byte[] pixels)
            : base(width, height, pixels)
        {
        }
    }
}