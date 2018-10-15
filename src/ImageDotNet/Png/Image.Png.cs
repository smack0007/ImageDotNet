using System;
using System.IO;
using System.IO.Compression;

namespace ImageDotNet
{
    public abstract partial class Image
    {
        private static readonly byte[] HeaderBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        enum ChunkType
        {
            Other = -1,
            IHDR,
            PLTE,
            IDAT,
            IEND
        }

        public static Image LoadPng(string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return LoadPng(file);
            }
        }

        public static Image LoadPng(Stream stream)
        {
            var header = new byte[HeaderBytes.Length];
            stream.Read(header, 0, header.Length);

            for (int i = 0; i < HeaderBytes.Length; i++)
            {
                if (header[i] != HeaderBytes[i])
                    throw new ImageDotNetException("PNG header incorrect.");
            }

            int width = 0;
            int height = 0;
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

                var chunkSize = ReadInt(chunkHeader, 0);
                var chunkType = ReadChunkType(chunkHeader, 4);

                if (chunkType == ChunkType.IEND)
                {
                    break;
                }

                var chunkData = new byte[chunkSize];
                stream.Read(chunkData, 0, chunkSize);

                switch (chunkType)
                {
                    case ChunkType.IHDR:
                        width = ReadInt(chunkData, 0);
                        height = ReadInt(chunkData, 4);
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

                            case 6: // RGB
                                bytesPerPixel = 4;
                                break;
                        }

                        pixels = new byte[width * height * bytesPerPixel];
                        scanline = new byte[width * bytesPerPixel];
                        break;

                    case ChunkType.PLTE:

                        break;

                    case ChunkType.IDAT:
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
            }

            if (bytesPerPixel == 3)
            {
                return new RgbImage(width, height, pixels);
            }
            else
            {
                return new RgbaImage(width, height, pixels);
            }
        }

        private static int ReadInt(byte[] data, int offset)
        {
            return (data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3];
        }

        private static ChunkType ReadChunkType(byte[] data, int offset)
        {
            if (data[offset] == 'I')
            {
                if (data[offset + 1] == 'D' && data[offset + 2] == 'A' && data[offset + 3] == 'T')
                {
                    return ChunkType.IDAT;
                }
                else if (data[offset + 1] == 'E' && data[offset + 2] == 'N' && data[offset + 3] == 'D')
                {
                    return ChunkType.IEND;
                }
                else if (data[offset + 1] == 'H' && data[offset + 2] == 'D' && data[offset + 3] == 'R')
                {
                    return ChunkType.IHDR;
                }
            }
            else if (data[offset] == 'P' && data[offset + 1] == 'L' && data[offset + 2] == 'T' && data[offset + 3] == 'E')
            {
                return ChunkType.PLTE;
            }

            return ChunkType.Other;
        }
    }
}
