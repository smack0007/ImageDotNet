using System.IO;

namespace ImageDotNet
{
    public partial interface IImage
    {
        void SavePng(string fileName);

        void SavePng(Stream stream);
    }
}
