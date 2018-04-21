using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    public struct ImageDataPointer : IDisposable
    {
        private GCHandle handle;

        public IntPtr Pointer { get; }

        public int Length { get; }

        internal ImageDataPointer(GCHandle handle, int length)
        {
            this.handle = handle;
            this.Pointer = this.handle.AddrOfPinnedObject();
            this.Length = length;
        }

        public void Dispose()
        {
            this.handle.Free();
        }
    }
}
