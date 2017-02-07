namespace Magnesium.OpenGL
{
    public interface IGLImage : IMgImage
    {
        int Width { get; }
        int Height { get; }
        GLImageArraySubresource[] ArrayLayers { get; }
        int Levels { get; }
        int OriginalTextureId { get; }
        MgFormat Format { get; }
    }
}