using System;

namespace Magnesium.OpenGL.UnitTests
{
    interface IGLLunarUniformBindingEntrypoint
    {
        void BindUniformBuffer(uint binding, int bufferId, IntPtr offset, long size);
    }
}
