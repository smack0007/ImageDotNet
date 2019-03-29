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

        void ForEachPixel(PixelAction action);

        void FlipVertically();
    }
}
