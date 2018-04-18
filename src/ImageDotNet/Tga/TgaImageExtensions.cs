using System.IO;

namespace ImageDotNet.Tga
{
    public static class TgaImageExentions
    {
        public static void SaveTga(this Image image, string fileName, TgaDataType dataType = TgaDataType.UncompressedTrueColor)
        {
            using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                SaveTga(image, file, dataType);
            }
        }

        public static void SaveTga(this Image image, Stream stream, TgaDataType dataType = TgaDataType.UncompressedTrueColor) =>
            TgaImage.Save(image, stream, dataType);        
    }
}