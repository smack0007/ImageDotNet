namespace ImageDotNet
{
    internal static class BinaryHelper
    {
        public static ushort ReadLittleEndianUInt16(byte[] bytes, int offset)
        {
            return (ushort)((bytes[offset + 1] << 8) | bytes[offset]);
        }

        public static void WriteLittleEndianUInt16(byte[] bytes, int offset, ushort value)
        {
            bytes[offset] = (byte)(value & 0x00FF);
            bytes[offset + 1] = (byte)(value >> 8);
        }
    }
}