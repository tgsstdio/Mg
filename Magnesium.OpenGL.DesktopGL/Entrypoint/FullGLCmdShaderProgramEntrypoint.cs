using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLCmdShaderProgramEntrypoint : IGLCmdShaderProgramEntrypoint
    {
        public void BindCombinedImageSampler(int programID, uint binding, ulong value)
        {
            throw new NotImplementedException();
        }

        public void BindProgram(int programID)
        {
            throw new NotImplementedException();
        }

        public void BindStorageBuffer(uint binding, int bufferId, long offset, int size)
        {
            throw new NotImplementedException();
        }

        public void BindUniformBuffers(uint count, int[] buffers, IntPtr[] offsets, int[] sizes)
        {
            throw new NotImplementedException();
        }

        public void BindVAO(int vao)
        {
            throw new NotImplementedException();
        }

        public void SetUniformBlock(int programID, int activeIndex, uint bindingPoint)
        {
            throw new NotImplementedException();
        }
    }
}
