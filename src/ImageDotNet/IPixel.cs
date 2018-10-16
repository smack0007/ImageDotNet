using System;
using System.Collections.Generic;
using System.Text;

namespace ImageDotNet
{
    public interface IPixel
    {
        void ReadFrom(byte[] buffer, int offset);
    }
}
