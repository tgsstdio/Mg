using System;
using OpenTK.Graphics.OpenGL4;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLCmdShaderProgramEntrypoint : IGLCmdShaderProgramEntrypoint
    {
        public void BindCombinedImageSampler(int programID, int binding, long value)
        {
            GL.Arb.ProgramUniformHandle(programID, binding, value);
        }

        public void BindProgram(int programID)
        {
            GL.UseProgram(programID);
        }

        public void BindStorageBuffer(uint binding, uint bufferId, IntPtr offset, IntPtr size)
        {
            GL.BindBufferRange(BufferRangeTarget.ShaderStorageBuffer, binding, bufferId, offset, size);
        }

        public void BindUniformBuffers(int count, uint[] buffers, IntPtr[] offsets, IntPtr[] sizes)
        {
            GL.BindBuffersRange(BufferRangeTarget.UniformBuffer, 0, count, buffers, offsets, sizes);
        }

        public void BindVAO(uint vao)
        {
            GL.BindVertexArray(vao);
        }

        public void SetUniformBlock(int programID, int activeIndex, int bindingPoint)
        {
            GL.UniformBlockBinding(programID, activeIndex, bindingPoint);
        }
    }
}
