using System;

namespace ImageDotNet
{
    public partial interface IImage
    {
        Type PixelType { get; }

        int Width { get; }

        int Height { get; }

        int BytesPerPixel { get; }

        int Length { get; }

        ImageDataPointer GetDataPointer();

        void ForEachPixel(Action<IPixel> action);

        Image<Rgb24> ToRgb24();

        Image<Rgba32> ToRgba32();
    }
}
