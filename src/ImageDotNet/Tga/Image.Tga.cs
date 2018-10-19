using System.IO;
using ImageDotNet.Tga;

namespace ImageDotNet
{
    public static partial class Image
    {
        public static IImage LoadTga(string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return LoadTga(file);
            }
        }

        public static IImage LoadTga(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);

            var header = br.ReadBytes(TgaHeader.SizeInBytes);

            var dataType = (TgaDataType)header[TgaHeader.DataTypeCode];

            if (dataType != TgaDataType.UncompressedTrueColor && dataType != TgaDataType.RunLengthEncodedTrueColor)
                throw new ImageDotNetException($"Only {nameof(TgaDataType.UncompressedTrueColor)} and {nameof(TgaDataType.RunLengthEncodedTrueColor)} TGA images are supported.");

            ushort width = BinaryHelper.ReadLittleEndianUInt16(header, TgaHeader.Width);
            ushort height = BinaryHelper.ReadLittleEndianUInt16(header, TgaHeader.Height);
            byte bytesPerPixel = (byte)(header[TgaHeader.BitsPerPixel] / 8);

            if (bytesPerPixel != 3 && bytesPerPixel != 4)
                throw new ImageDotNetException("Only 24 and 32 bit TGA images are supported.");

            byte[] pixels = null;
            int dataLength = width * height * bytesPerPixel;

            if (dataType == TgaDataType.UncompressedTrueColor)
            {
                pixels = br.ReadBytes(dataLength);
            }
            else
            {
                pixels = new byte[dataLength];

                int readCount = 0;
                while (readCount < dataLength)
                {
                    byte nextByte = br.ReadByte();

                    if ((nextByte & 0x80) != 0) // Check high bit
                    {
                        nextByte -= 127;
                        byte[] bytes = br.ReadBytes(bytesPerPixel);

                        for (int i = 0; i < nextByte; i++)
                        {
                            for (int j = 0; j < bytes.Length; j++)
                            {
                                pixels[readCount] = bytes[j];
                                readCount++;
                            }
                        }
                    }
                    else // Raw chunk
                    {
                        nextByte += 1;
                        byte[] bytes = br.ReadBytes(nextByte * bytesPerPixel);

                        for (int i = 0; i < bytes.Length; i++)
                        {
                            pixels[readCount] = bytes[i];
                            readCount++;
                        }
                    }
                }
            }

            pixels = pixels.FlipVertically(width, height, bytesPerPixel);

            if (bytesPerPixel == 3)
            {
                return new Image<Bgr24>(width, height, PixelHelper.ToPixelArray<Bgr24>(pixels));
            }
            else
            {
                return new Image<Bgra32>(width, height, PixelHelper.ToPixelArray<Bgra32>(pixels));
            }
        }
    }

    public partial class Image<T>
    {
        public void SaveTga(string fileName, TgaDataType dataType = TgaDataType.UncompressedTrueColor)
        {
            using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                SaveTga(file, dataType);
            }
        }

        public void SaveTga(Stream stream, TgaDataType dataType = TgaDataType.UncompressedTrueColor)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            var header = new byte[TgaHeader.SizeInBytes];
            header[TgaHeader.DataTypeCode] = (byte)dataType;
            BinaryHelper.WriteLittleEndianUInt16(header, TgaHeader.Width, (ushort)Width);
            BinaryHelper.WriteLittleEndianUInt16(header, TgaHeader.Height, (ushort)Height);
            header[TgaHeader.BitsPerPixel] = (byte)(BytesPerPixel * 8);

            bw.Write(header);

            byte[] pixels = null;
            if (BytesPerPixel == 3)
            {
                pixels = PixelHelper.Clone(_pixels)
                    .Convert<T, Bgr24>()
                    .FlipVertically(Width, Height)
                    .ToByteArray();
            }
            else if (BytesPerPixel == 4)
            {
                pixels = PixelHelper.Clone(_pixels)
                    .Convert<T, Bgra32>()
                    .FlipVertically(Width, Height)
                    .ToByteArray();
            }

            bw.Write(pixels);

            bw.Flush();
        }
    }
}
