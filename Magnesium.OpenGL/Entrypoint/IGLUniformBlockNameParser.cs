namespace Magnesium.OpenGL
{
    public interface IGLUniformBlockNameParser
    {
        GLUniformBlockInfo Parse(string name);
    }
}