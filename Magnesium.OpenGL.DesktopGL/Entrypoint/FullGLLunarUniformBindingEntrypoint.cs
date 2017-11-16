using OpenTK.Graphics.OpenGL4;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLLunarUniformBindingEntrypoint : IGLLunarUniformBindingEntrypoint
    {
        private readonly IGLErrorHandler mErrHandler;
        public FullGLLunarUniformBindingEntrypoint(IGLErrorHandler errHandler)
        {
            mErrHandler = errHandler;
        }

        public void BindUniformBuffer(uint binding, uint bufferId, IntPtr offset, long size)
        {
            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, binding, bufferId, offset, (int) size);
            mErrHandler.LogGLError("BindUniformBuffers");
        }
    }
}
