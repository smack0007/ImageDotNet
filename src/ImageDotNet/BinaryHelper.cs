using System;
using System.Linq;

namespace ImageDotNet
{
    internal static class BinaryHelper
    {
        enum Endianess { Big, Little };

        private static byte[] GetBytes(Endianess endianess, ushort value)
        {
            var bytes = BitConverter.GetBytes(value);

            if ((endianess == Endianess.Big && BitConverter.IsLittleEndian) ||
                (endianess == Endianess.Little && !BitConverter.IsLittleEndian))
            {
                bytes = bytes.Reverse().ToArray();
            }

            return bytes;
        }

        private static byte[] GetBytes(Endianess endianess, uint value)
        {
            var bytes = BitConverter.GetBytes(value);

            if ((endianess == Endianess.Big && BitConverter.IsLittleEndian) ||
                (endianess == Endianess.Little && !BitConverter.IsLittleEndian))
            {
                bytes = bytes.Reverse().ToArray();
            }

            return bytes;
        }

        public static uint ReadBigEndianUInt32(byte[] data, int offset)
        {
            return (uint)(data[offset] << 24) | (uint)(data[offset + 1] << 16) | (uint)(data[offset + 2] << 8) | data[offset + 3];
        }

        public static ushort ReadLittleEndianUInt16(byte[] bytes, int offset)
        {
            return (ushort)((bytes[offset + 1] << 8) | bytes[offset]);
        }

        public static void WriteBigEndianUInt32(byte[] buffer, int offset, uint value)
        {
            var bytes = GetBytes(Endianess.Big, value);

            buffer[offset] = bytes[0];
            buffer[offset + 1] = bytes[1];
            buffer[offset + 2] = bytes[2];
            buffer[offset + 3] = bytes[3];
        }

        public static void WriteLittleEndianUInt16(byte[] buffer, int offset, ushort value)
        {
            var bytes = GetBytes(Endianess.Little, value);

            buffer[offset] = bytes[0];
            buffer[offset + 1] = bytes[1];
        }
    }
}
