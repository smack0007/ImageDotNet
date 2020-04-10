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

        public static uint ReadBigEndianUInt32(byte[] buffer, int offset)
        {
            var bytes = buffer.Skip(offset).Take(4).ToArray();

            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            return BitConverter.ToUInt32(bytes, 0);
        }

        public static ushort ReadLittleEndianUInt16(byte[] buffer, int offset)
        {
            var bytes = buffer.Skip(offset).Take(2).ToArray();

            if (!BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            return BitConverter.ToUInt16(bytes, 0);
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
