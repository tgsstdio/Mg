using System;

namespace Magnesium.OpenGL
{
    public interface IGLBlitOperationEntrypoint
    {
        void Initialize();
        void CopyBuffer(uint src, uint dst, IntPtr readOffset, IntPtr writeOffset, int size);
        void PerformOperation(GLCmdImageInstructionSet instructionSet);
    }
}
