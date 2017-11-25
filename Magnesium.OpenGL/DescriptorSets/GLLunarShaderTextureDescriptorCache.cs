using System;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
    public class GLLunarShaderTextureDescriptorCache : IGLShaderTextureDescriptorCache
    {
        private IGLExtensionLookup mLookup;
        private IGLLunarUniformBindingEntrypoint mEntrypoint;
        public GLLunarShaderTextureDescriptorCache(IGLExtensionLookup lookup, IGLLunarUniformBindingEntrypoint entrypoint)
        {
            mLookup = lookup;
            mEntrypoint = entrypoint;
        }

        public void Bind(IGLFutureDescriptorSet ds, GLDescriptorPoolResourceInfo resource)
        {
            var parentPool = (GLLunarDescriptorPool) ds.Parent;
            Debug.Assert(parentPool != null);

            var bufferId = parentPool.BufferId;
            const int HANDLE_SIZE = sizeof(long);
            var offset = new IntPtr(HANDLE_SIZE * resource.Ticket.First);
            var size = HANDLE_SIZE * resource.DescriptorCount;            

            mEntrypoint.BindUniformBuffer(resource.Binding, bufferId, offset, size);
        }

        public void Initialize()
        {
            if (!mLookup.HasExtension("GL_ARB_bindless_texture"))
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}
