using System.Collections.Generic;

namespace Magnesium.OpenGL.UnitTests
{
    public class MockTextureCentralEntrypoint : IGLTextureGalleryEntrypoint
    {
        public class ViewInfo
        {
            public MgImageViewType ViewType { get; set; }
            public int TextureId { get; set; }
        }

        public Dictionary<uint, ViewInfo> Views = new Dictionary<uint, ViewInfo>();
        public void BindView(uint binding, MgImageViewType viewType, int texture)
        {
            if (Views.TryGetValue(binding, out ViewInfo view))
            {
                view.TextureId = texture;
                view.ViewType = viewType;
            }
            else
            {
                Views.Add(binding, new ViewInfo { TextureId = texture, ViewType = viewType });
            }            
        }

        public void UnbindView(uint binding, MgImageViewType viewType)
        {
            if (Views.TryGetValue(binding, out ViewInfo view))
            {
                view.TextureId = 0;
                view.ViewType = viewType;
            }
            else
            {
                Views.Add(binding, new ViewInfo { TextureId = 0, ViewType = viewType });
            }
        }

        public Dictionary<uint, int> Samplers = new Dictionary<uint, int>();
        public void BindSampler(uint binding, int sampler)
        {
            if (Samplers.ContainsKey(binding))
            {
                Samplers[binding] = sampler;
            }
            else
            {
                Samplers.Add(binding, sampler);
            }            
        }

        public int MaxSlots { get; set; }
        public int GetMaximumNumberOfTextureUnits()
        {
            return MaxSlots;
        }

        public void UnbindSampler(uint binding)
        {
            if (Samplers.ContainsKey(binding))
            {
                Samplers[binding] = 0;
            }
            else
            {
                Samplers.Add(binding, 0);
            }
        }
    }
}

