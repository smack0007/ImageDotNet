using System.IO;
using ImageDotNet.Tga;

namespace ImageDotNet
{
    public partial interface IImage
    {
        void SaveTga(string fileName, TgaDataType dataType = TgaDataType.UncompressedTrueColor);

        void SaveTga(Stream stream, TgaDataType dataType = TgaDataType.UncompressedTrueColor);
    }
}
