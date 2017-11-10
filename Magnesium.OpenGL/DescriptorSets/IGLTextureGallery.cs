namespace Magnesium.OpenGL
{
    public interface IGLTextureGallery
    {
        GLTextureSlot[] AvailableSlots { get; }

        void Bind(GLTextureSlot[] descriptors);
        void Initialize();
    }
}