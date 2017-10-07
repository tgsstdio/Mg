using System.IO;

namespace TriangleDemo
{
    public interface ITriangleDemoShaderPath
    {
        Stream OpenVertexShader();
        Stream OpenFragmentShader();
    }
}