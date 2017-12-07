using System.IO;

namespace OffscreenDemo
{
    public interface IRenderToTexturePipelineMediaPath
    {
        Stream OpenVertexShader();
        Stream OpenFragmentShader();
    }
}