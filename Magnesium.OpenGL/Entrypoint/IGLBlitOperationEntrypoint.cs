using System;

namespace Magnesium.OpenGL
{
    public interface IGLBlitOperationEntrypoint
    {
        void CopyBuffer(int src, int dst, IntPtr readOffset, IntPtr writeOffset, int size);
    }
}
