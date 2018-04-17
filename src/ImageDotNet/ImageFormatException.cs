using System;

namespace ImageDotNet
{
    public class ImageFormatException : Exception
    {
        public ImageFormatException(string message)
            : base(message)
        {
        }
    }
}
