namespace ImageDotNet
{
    public delegate void PixelAction(ref IPixel pixel);

    public delegate void PixelAction<T>(ref T pixel) where T : unmanaged, IPixel;
}
