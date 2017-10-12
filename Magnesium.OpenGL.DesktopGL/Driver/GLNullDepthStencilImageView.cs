namespace Magnesium.OpenGL.DesktopGL.Internals
{
    public class GLNullDepthStencilImageView : IMgImageView
    {
        public MgFormat Format { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }

        public void DestroyImageView(IMgDevice device, IMgAllocationCallbacks allocator)
        {

        }
    }
}