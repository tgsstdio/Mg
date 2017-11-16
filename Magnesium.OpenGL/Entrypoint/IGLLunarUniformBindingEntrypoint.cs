using System;

namespace Magnesium.OpenGL
{
    public interface IGLLunarUniformBindingEntrypoint
    {
        void BindUniformBuffer(uint binding, uint bufferId, IntPtr offset, long size);
    }
}
