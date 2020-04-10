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

        bool Is<T>() where T : unmanaged, IPixel;

        Image<T> To<T>() where T : unmanaged, IPixel;

        T[] ToPixelArray<T>() where T : unmanaged, IPixel;

        void FlipVertically();
    }
}
