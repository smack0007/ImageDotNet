using System.IO;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    internal static class BinaryHelper
    {
        public static T ReadStruct<T>(this BinaryReader br)
            where T: struct
        {
            var sizeInBytes = Marshal.SizeOf(typeof(T));
            var buffer = new byte[sizeInBytes]; 

            br.Read(buffer, 0, sizeInBytes);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

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