using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.DesktopGL.Internals
{
    public class GLNullDepthStencilImageView : IGLImageView
    {
        public MgFormat Format { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }

        public int TextureId { get => 0; }

        public MgImageViewType ViewTarget { get => MgImageViewType.TYPE_2D; }

        public bool IsNullImage { get => true; }

        public void DestroyImageView(IMgDevice device, IMgAllocationCallbacks allocator)
        {

        }
    }
}