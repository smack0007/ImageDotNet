/// <auto-generated />
using System;

namespace ImageDotNet
{
    public enum PixelFormat
    {
        Bgr24,
        Bgra32,
        Rgb24,
        Rgba32,
    }

    public static partial class PixelHelper
    {
        public static Type ToPixelType(this PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Bgr24: return typeof(Bgr24);
                case PixelFormat.Bgra32: return typeof(Bgra32);
                case PixelFormat.Rgb24: return typeof(Rgb24);
                case PixelFormat.Rgba32: return typeof(Rgba32);
            }

            throw new ImageDotNetException($"Unknown {nameof(PixelFormat)}.");
        }

        public static PixelFormat ToPixelFormat<T>() where T: IPixel => ToPixelFormat(typeof(T));

        public static PixelFormat ToPixelFormat(Type pixelType)
        {
            if (pixelType == typeof(Bgr24)) return PixelFormat.Bgr24;
            if (pixelType == typeof(Bgra32)) return PixelFormat.Bgra32;
            if (pixelType == typeof(Rgb24)) return PixelFormat.Rgb24;
            if (pixelType == typeof(Rgba32)) return PixelFormat.Rgba32;

            throw new ImageDotNetException($"Unknown {nameof(PixelFormat)}.");
        }
    }
}