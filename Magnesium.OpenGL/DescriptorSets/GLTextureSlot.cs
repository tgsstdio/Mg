using System;
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL
{
    public class GLTextureSlot
    {
        public uint Binding { get; set; }
        public IGLImageView View { get; set; }
        public IGLSampler Sampler { get; set; }

        public void Replace(uint dstBinding, IGLImageView localView, IGLSampler localSampler)
        {
            Binding = dstBinding;
            View = localView;
            Sampler = localSampler;
        }
    }
}

