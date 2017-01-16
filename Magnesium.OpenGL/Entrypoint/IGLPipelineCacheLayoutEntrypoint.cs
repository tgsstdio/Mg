using System;

namespace Magnesium.OpenGL
{
    public interface IGLPipelineCacheLayoutEntrypoint
    {
        int GetMaximumNoOfUniformBindings();
        int GetMaximumNoOfSSBOBindings();

        void BindBufferRange(MgBufferUsageFlagBits usage, int index, int buffer, IntPtr offset, ulong size);
    }
}
