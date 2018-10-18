using System;
using System.Collections.Generic;
using System.Text;

namespace ImageDotNet
{
    internal static class Guard
    {
        public static void NotNull<T>(T value, string name)
            where T: class
        {
            if (value is null)
                throw new ArgumentNullException(name);
        }
    }
}
