using System.IO;

namespace TextureDemo
{
    public interface ITriangleDemoShaderPath
    {
        Stream OpenVertexShader();
        Stream OpenFragmentShader();
    }
}