namespace Magnesium.OpenGL.Internals
{
    public interface IGLImageView : IMgImageView
    {
        int TextureId { get; }
        MgImageViewType ViewTarget { get; }
        bool IsNullImage { get; }
    }
}

