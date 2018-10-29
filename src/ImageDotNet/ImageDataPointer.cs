using System;
using System.Runtime.InteropServices;

namespace ImageDotNet
{
    public struct ImageDataPointer : IDisposable
    {
        private GCHandle _handle;

        public IntPtr Pointer { get; }

        public int Length { get; }

        internal ImageDataPointer(GCHandle handle, int length)
        {
            _handle = handle;
            Pointer = _handle.AddrOfPinnedObject();
            Length = length;
        }

        public void Dispose()
        {
            _handle.Free();
        }
    }
}
