using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLBlitOperationEntrypoint : IGLBlitOperationEntrypoint
    {
        public void CopyBuffer(uint src, uint dst, IntPtr readOffset, IntPtr writeOffset, int size)
        {
            GL.CopyNamedBufferSubData(src, dst, readOffset, writeOffset, size);
        }
    }
}
