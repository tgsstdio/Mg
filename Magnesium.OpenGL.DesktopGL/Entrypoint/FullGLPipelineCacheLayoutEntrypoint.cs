using OpenTK.Graphics.OpenGL4;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    class FullGLPipelineCacheLayoutEntrypoint : IGLPipelineCacheLayoutEntrypoint
    {
        public void BindBufferRange(MgBufferUsageFlagBits usage, int index, int buffer, IntPtr offset, ulong size)
        {
          //  GL.BindBufferRange();
        }

        public int GetMaximumNoOfSSBOBindings()
        {
            int result;
            GL.GetInteger((GetPName) All.MaxShaderStorageBufferBindings, out result);
            return 0;
        }

        public int GetMaximumNoOfUniformBindings()
        {
            int result;
            GL.GetInteger(GetPName.MaxUniformBufferBindings, out result);
            return result;
        }
    }
}
