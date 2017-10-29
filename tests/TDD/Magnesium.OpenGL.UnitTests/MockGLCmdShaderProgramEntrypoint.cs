using System;

namespace Magnesium.OpenGL.UnitTests
{
    internal class MockGLCmdShaderProgramEntrypoint : IGLCmdShaderProgramEntrypoint
    {
        public MockGLCmdShaderProgramEntrypoint()
        {
        }

        public void BindCombinedImageSampler(int programID, int binding, long value)
        {
            throw new NotImplementedException();
        }

        public void BindProgram(int programID)
        {
            
        }

        public void BindStorageBuffer(uint binding, uint bufferId, IntPtr offset, IntPtr size)
        {

        }

        public int Count { get; set; }
        public uint[] UniformBuffers { get; private set; }
        public IntPtr[] UniformOffsets { get; private set; }
        public IntPtr[] UniformSizes { get; private set; }

        public void BindUniformBuffers(int count, uint[] buffers, IntPtr[] offsets, IntPtr[] sizes)
        {
            // LEFT BLANK
            Count = count;
            UniformBuffers = buffers;
            UniformOffsets = offsets;
            UniformSizes = sizes;
        }

        public void BindVAO(uint vao)
        {
            throw new NotImplementedException();
        }

        public int ProgramID { get; set; }
        public int NoOfSetUniformCalls { get; set; }

        public void SetUniformBlock(int programID, int activeIndex, int bindingPoint)
        {
            ProgramID = programID;
            NoOfSetUniformCalls += 1;
        }
    }
}