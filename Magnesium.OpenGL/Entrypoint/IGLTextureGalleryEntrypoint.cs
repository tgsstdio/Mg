namespace Magnesium.OpenGL
{
    public interface IGLTextureGalleryEntrypoint
    {
        int GetMaximumNumberOfTextureUnits();

        void BindView(uint binding, MgImageViewType viewType, int texture);
        void UnbindView(uint binding, MgImageViewType viewType);
        void BindSampler(uint binding, int sampler);
        void UnbindSampler(uint binding);
    }
}

