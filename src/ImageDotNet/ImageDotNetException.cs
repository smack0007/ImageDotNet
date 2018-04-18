using System;

namespace ImageDotNet
{
    public class ImageDotNetException : Exception
    {
        public ImageDotNetException(string message)
            : base(message)
        {
        }
    }
}
