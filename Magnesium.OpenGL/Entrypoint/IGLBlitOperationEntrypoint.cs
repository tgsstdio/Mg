using System;

namespace Magnesium.OpenGL
{
    public interface IGLBlitOperationEntrypoint
    {
        void CopyBuffer(uint src, uint dst, IntPtr readOffset, IntPtr writeOffset, int size);
    }
}
