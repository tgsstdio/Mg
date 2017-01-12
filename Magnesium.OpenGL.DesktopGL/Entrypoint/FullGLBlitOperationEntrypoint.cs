using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLBlitOperationEntrypoint : IGLBlitOperationEntrypoint
    {
        public void CopyBuffer(int src, int dst, IntPtr readOffset, IntPtr writeOffset, int size)
        {
            GL.CopyNamedBufferSubData(src, dst, readOffset, writeOffset, size);
        }
    }
}
