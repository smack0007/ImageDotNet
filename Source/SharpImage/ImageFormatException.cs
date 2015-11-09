using System;

namespace SharpImage
{
    public class ImageFormatException : Exception
    {
        public ImageFormatException(string message)
            : base(message)
        {
        }
    }
}
