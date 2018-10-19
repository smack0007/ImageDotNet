namespace ImageDotNet
{
    internal static class BinaryHelper
    {
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
            buffer[offset] = (byte)(value & (0xFF << 24));
            buffer[offset + 1] = (byte)(value & (0xFF << 16));
            buffer[offset + 2] = (byte)(value & (0xFF << 8));
            buffer[offset + 3] = (byte)(value & (0xFF));
        }

        public static void WriteLittleEndianUInt16(byte[] bytes, int offset, ushort value)
        {
            bytes[offset] = (byte)(value & 0x00FF);
            bytes[offset + 1] = (byte)(value >> 8);
        }
    }
}
