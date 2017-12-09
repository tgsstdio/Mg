using System;
using OpenTK.Graphics.OpenGL4;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLCmdShaderProgramEntrypoint : IGLCmdShaderProgramEntrypoint
    {
        private IGLErrorHandler mErrHandler;
        private IGLFramebufferHelperSelector mSelector;
        public FullGLCmdShaderProgramEntrypoint(IGLErrorHandler errHandler, IGLFramebufferHelperSelector selector)
        {
            mErrHandler = errHandler;
            mSelector = selector;
        }

        public void BindCombinedImageSampler(int programID, int binding, long value)
        {
            GL.Arb.ProgramUniformHandle(programID, binding, value);
            mErrHandler.LogGLError("BindCombinedImageSampler");
        }

        public void BindFramebuffer(int fbo)
        {
            mSelector.Helper.BindFramebuffer(fbo);
        }

        public void BindProgram(int programID)
        {
            GL.UseProgram(programID);
            mErrHandler.LogGLError("BindProgram");
        }

        public void BindStorageBuffer(uint binding, uint bufferId, IntPtr offset, IntPtr size)
        {
            GL.BindBufferRange(BufferRangeTarget.ShaderStorageBuffer, binding, bufferId, offset, size);
            mErrHandler.LogGLError("BindStorageBuffer");
        }

        public void BindUniformBuffer(uint binding, uint bufferId, IntPtr offset, IntPtr size)
        {
            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, binding, bufferId, offset, size);
            mErrHandler.LogGLError("BindUniformBuffer");
        }

        public void BindUniformBuffers(int count, uint[] buffers, IntPtr[] offsets, IntPtr[] sizes)
        {
            GL.BindBuffersRange(BufferRangeTarget.UniformBuffer, 0, count, buffers, offsets, sizes);
            mErrHandler.LogGLError("BindUniformBuffers");
        }

        public void BindVAO(uint vao)
        {
            GL.BindVertexArray(vao);
            mErrHandler.LogGLError("BindVAO");
        }

        public void SetUniformBlock(int programID, int activeIndex, int bindingPoint)
        {
            GL.UniformBlockBinding(programID, activeIndex, bindingPoint);
            mErrHandler.LogGLError("SetUniformBlock");
        }
    }
}
