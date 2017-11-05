using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLBlitOperationEntrypoint : IGLBlitOperationEntrypoint
    {
        private IGLErrorHandler mErrHandler;
        public FullGLBlitOperationEntrypoint(IGLErrorHandler errHandler)
        {
            mErrHandler = errHandler;
        }

        public void CopyBuffer(uint src, uint dst, IntPtr readOffset, IntPtr writeOffset, int size)
        {
            GL.CopyNamedBufferSubData(src, dst, readOffset, writeOffset, size);
            mErrHandler.LogGLError("CopyBuffer");
        }
    }
}
