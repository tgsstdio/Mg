using System.Diagnostics;

namespace Magnesium.OpenGL
{
    class GLSolarShaderTextureDescriptorCache : IGLShaderTextureDescriptorCache
    {
        private IGLTextureGalleryEntrypoint mEntrypoint;
        private GLTextureSlot[] AvailableSlots;

        public GLSolarShaderTextureDescriptorCache(IGLTextureGalleryEntrypoint entrypoint)
        {
            mEntrypoint = entrypoint;
            AvailableSlots = new GLTextureSlot[0];
        }

        public void Initialize()
        {
            var maxSlots = mEntrypoint.GetMaximumNumberOfTextureUnits();
            AvailableSlots = new GLTextureSlot[maxSlots];
            for (var i = 0U; i < maxSlots; i += 1)
            {
                AvailableSlots[i] = new GLTextureSlot
                {
                    Binding = i,
                    Sampler = null,
                    View = null,
                };
                // REMOVE ALL EXISTING BINDINGS 
                // NOT SURE WHICH VALUE SHOULD BE USED
                mEntrypoint.UnbindView(i, MgImageViewType.TYPE_2D);
                mEntrypoint.UnbindSampler(i);
            }
        }

        public void Bind(IGLFutureDescriptorSet ds, GLDescriptorPoolResourceInfo resource)
        {
            var parentPool = (GLSolarDescriptorPool) ds.Parent;
            Debug.Assert(parentPool != null);

            for (var i = resource.Ticket.First; i <= resource.Ticket.Last; i += 1)
			{
                BindImages(parentPool.CombinedImageSamplers.Items[i]);
			}
        }

        void BindImages(GLTextureSlot srcDescriptor)
        {
            var destBinding = srcDescriptor.Binding;

            if (destBinding < (uint)AvailableSlots.Length)
            {
                var dest = AvailableSlots[destBinding];

                UpdateViewIfNeeded(srcDescriptor, destBinding, dest);

                UpdateSamplerIfNeeded(dest, destBinding, srcDescriptor);
            }            
        }

        private void UpdateSamplerIfNeeded(GLTextureSlot dest, uint destBinding, GLTextureSlot srcDescriptor)
        {
            bool needsUpdate = false;
            if (dest.Sampler != null)
            {
                if (srcDescriptor.Sampler != null
                    && dest.Sampler.SamplerId != srcDescriptor.Sampler.SamplerId)
                {
                    mEntrypoint.BindSampler(destBinding, srcDescriptor.Sampler.SamplerId);
                    needsUpdate = true;
                }
                else if (srcDescriptor.Sampler == null)
                {
                    mEntrypoint.UnbindSampler(destBinding);
                    needsUpdate = true;
                }
            }
            else if (srcDescriptor.Sampler != null)
            {
                mEntrypoint.BindSampler(destBinding, srcDescriptor.Sampler.SamplerId);
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                dest.Sampler = srcDescriptor.Sampler;
            }
        }

        private void UpdateViewIfNeeded(GLTextureSlot srcDescriptor, uint destBinding, GLTextureSlot dest)
        {
            bool needsUpdate = false;
            if (dest.View != null)
            {
                if (
                    srcDescriptor.View != null
                    && (
                        dest.View.ViewTarget != srcDescriptor.View.ViewTarget
                    || dest.View.TextureId != srcDescriptor.View.TextureId
                    )
                )
                {
                    var view = srcDescriptor.View;
                    mEntrypoint.BindView(destBinding, view.ViewTarget, view.TextureId);
                    needsUpdate = true;
                }
                else if (srcDescriptor.View == null)
                {
                    mEntrypoint.UnbindView(destBinding, dest.View.ViewTarget);
                    needsUpdate = true;
                }
            }
            else if (srcDescriptor.View != null)
            {
                var view = srcDescriptor.View;
                mEntrypoint.BindView(destBinding, view.ViewTarget, view.TextureId);
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                dest.View = srcDescriptor.View;
            }
        }
    }
}
