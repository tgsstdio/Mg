using System;

namespace Magnesium.OpenGL
{
    public interface IGLFutureDeviceMemoryEntrypoint
    {
        uint CreateBufferStorage(int sizeInBytes, uint flags);
        IntPtr MapBufferStorage(uint bufferId, IntPtr offset, int size, uint flags);
        void UnmapBufferStorage(uint bufferId);
        ulong GetMinAlignment();
    }
}
