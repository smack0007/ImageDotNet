using System;
using System.IO;
using System.IO.Compression;
using ImageDotNet.Png;

namespace ImageDotNet
{
    public static partial class Image
    {
        public static IImage LoadPng(string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return LoadPng(file);
            }
        }

        public static IImage LoadPng(Stream stream)
        {
            var header = new byte[PngHelper.HeaderBytes.Length];
            stream.Read(header, 0, header.Length);

            for (int i = 0; i < PngHelper.HeaderBytes.Length; i++)
            {
                if (header[i] != PngHelper.HeaderBytes[i])
                    throw new ImageDotNetException("PNG header incorrect.");
            }

            uint width = 0;
            uint height = 0;
            byte bitDepth = 0;
            byte colorType = 0;
            byte compressionMethod = 0;
            byte filterMethod = 0;
            byte interlaceMethod = 0;

            int bytesPerPixel = 0;
            byte[] pixels = null;
            int pixelsOffset = 0;
            byte[] scanline = null;

            while (true)
            {
                var chunkHeader = new byte[8];
                stream.Read(chunkHeader, 0, chunkHeader.Length);

                var chunkSize = BinaryHelper.ReadBigEndianUInt32(chunkHeader, 0);
                var chunkType = ReadChunkType(chunkHeader, 4);

                if (chunkType == PngHelper.ChunkType.IEND)
                {
                    break;
                }

                var chunkData = new byte[chunkSize];
                stream.Read(chunkData, 0, (int)chunkSize);

                switch (chunkType)
                {
                    case PngHelper.ChunkType.IHDR:
                        width = BinaryHelper.ReadBigEndianUInt32(chunkData, 0);
                        height = BinaryHelper.ReadBigEndianUInt32(chunkData, 4);
                        bitDepth = chunkData[8];
                        colorType = chunkData[9];
                        compressionMethod = chunkData[10];
                        filterMethod = chunkData[11];
                        interlaceMethod = chunkData[12];

                        switch (colorType)
                        {
                            case 2: // RGB
                                bytesPerPixel = 3;
                                break;

                            case 6: // RGBA
                                bytesPerPixel = 4;
                                break;
                        }

                        pixels = new byte[width * height * bytesPerPixel];
                        scanline = new byte[width * bytesPerPixel];
                        break;

                    case PngHelper.ChunkType.PLTE:

                        break;

                    case PngHelper.ChunkType.IDAT:
                        using (MemoryStream chunkDataStream = new MemoryStream(chunkData))
                        {
                            // Read past the first two bytes of the zlib header
                            chunkDataStream.Seek(2, SeekOrigin.Begin);

                            using (var deflate = new DeflateStream(chunkDataStream, CompressionMode.Decompress))
                            {
                                for (int i = 0; i < height; i++)
                                {
                                    var scanlineFilterAlgorithm = deflate.ReadByte();
                                    deflate.Read(scanline, 0, scanline.Length);

                                    // TODO: Reverse scanline filter algorithm

                                    Buffer.BlockCopy(scanline, 0, pixels, pixelsOffset, scanline.Length);
                                    pixelsOffset += scanline.Length;
                                }
                            }
                        }
                                              
                        break;
                }

                var chunkCrc = new byte[4];
                stream.Read(chunkCrc, 0, 4);

                var crc = BinaryHelper.ReadBigEndianUInt32(chunkCrc, 0);
            }

            if (bytesPerPixel == 3)
            {
                return new Image<Rgb24>((int)width, (int)height, PixelHelper.ToPixelArray<Rgb24>(pixels));
            }
            else
            {
                return new Image<Rgba32>((int)width, (int)height, PixelHelper.ToPixelArray<Rgba32>(pixels));
            }
        }

        private static PngHelper.ChunkType ReadChunkType(byte[] data, int offset)
        {
            if (data[offset] == 'I')
            {
                if (data[offset + 1] == 'D' && data[offset + 2] == 'A' && data[offset + 3] == 'T')
                {
                    return PngHelper.ChunkType.IDAT;
                }
                else if (data[offset + 1] == 'E' && data[offset + 2] == 'N' && data[offset + 3] == 'D')
                {
                    return PngHelper.ChunkType.IEND;
                }
                else if (data[offset + 1] == 'H' && data[offset + 2] == 'D' && data[offset + 3] == 'R')
                {
                    return PngHelper.ChunkType.IHDR;
                }
            }
            else if (data[offset] == 'P' && data[offset + 1] == 'L' && data[offset + 2] == 'T' && data[offset + 3] == 'E')
            {
                return PngHelper.ChunkType.PLTE;
            }

            return PngHelper.ChunkType.Other;
        }        
    }

    public sealed partial class Image<T>
    {
        public void SavePng(string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                this.SavePng(file);
            }
        }

        public void SavePng(Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(PngHelper.HeaderBytes);

            byte[] ihdr = new byte[13];
            BinaryHelper.WriteBigEndianUInt32(ihdr, 0, (uint)Width);
            BinaryHelper.WriteBigEndianUInt32(ihdr, 4, (uint)Height);
            ihdr[8] = 8; // bitDepth
            ihdr[9] = GetColorType();
            ihdr[10] = 0; // compressionMethod
            ihdr[11] = 0; // filterMethod
            ihdr[12] = 0; // interlaceMethod
            WriteChunk(bw, "IHDR", ihdr);

            var scanline = new byte[Width * BytesPerPixel + 1]; // Add in an extra byte per scanline
            scanline[0] = 0;

            byte[] idat = null;

            using (var pixels = PixelData.Clone(_pixels))
            {
                if (BytesPerPixel == 3)
                {
                    pixels.Convert<Rgb24>();
                }
                else
                {
                    pixels.Convert<Rgba32>();
                }

                using (var ms = new MemoryStream(pixels.Length + 2)) // Add 2 bytes for zlib header
                {
                    // Don't know if the zlib header has to be 24 and 87 but it
                    // seems to work.
                    ms.WriteByte(24);
                    ms.WriteByte(87);

                    using (var deflate = new DeflateStream(ms, CompressionLevel.Optimal))
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                for (int i = 0; i < BytesPerPixel; i++)
                                    scanline[(x * BytesPerPixel) + i + 1] = pixels[(y * Width * BytesPerPixel) + (x * BytesPerPixel) + i];
                            }

                            deflate.Write(scanline, 0, scanline.Length);
                        }

                        deflate.Flush();
                        idat = ms.ToArray();
                    }
                }
            }

            WriteChunk(bw, "IDAT", idat);

            WriteChunk(bw, "IEND", Array.Empty<byte>());

            bw.Flush();
        }

        private byte GetColorType()
        {
            if (BytesPerPixel == 3)
            {
                return 2;
            }
            else
            {
                return 6;
            }
        }

        private static void WriteChunk(BinaryWriter bw, string chunkType, byte[] chunkData)
        {
            var chunkHeader = new byte[8];
            BinaryHelper.WriteBigEndianUInt32(chunkHeader, 0, (uint)chunkData.Length);

            for (int i = 0; i < chunkType.Length; i++)
                chunkHeader[i + 4] = (byte)chunkType[i];

            bw.Write(chunkHeader);

            bw.Write(chunkData);

            uint crc = CalculateCrc(chunkHeader, 4, 4, 0);
            crc = CalculateCrc(chunkData, 0, chunkData.Length, crc);

            byte[] chunkCrc = new byte[4];
            BinaryHelper.WriteBigEndianUInt32(chunkCrc, 0, crc);

            bw.Write(chunkCrc);
        }

        private static uint[] crcTable;

        private static uint CalculateCrc(byte[] buffer, int offset, int length, uint crc)
        {
            uint c;
            if (crcTable == null)
            {
                crcTable = new uint[256];
                for (uint n = 0; n <= 255; n++)
                {
                    c = n;
                    for (var k = 0; k <= 7; k++)
                    {
                        if ((c & 1) == 1)
                            c = 0xEDB88320 ^ ((c >> 1) & 0x7FFFFFFF);
                        else
                            c = ((c >> 1) & 0x7FFFFFFF);
                    }
                    crcTable[n] = c;
                }
            }

            c = crc ^ 0xffffffff;
            var endOffset = offset + length;
            for (var i = offset; i < endOffset; i++)
            {
                c = crcTable[(c ^ buffer[i]) & 255] ^ ((c >> 8) & 0xFFFFFF);
            }
            return c ^ 0xffffffff;
        }
    }
}
