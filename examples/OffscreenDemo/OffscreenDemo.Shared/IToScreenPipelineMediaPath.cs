using System.IO;

namespace OffscreenDemo
{
    public interface IToScreenPipelineMediaPath
    {
        Stream OpenVertexShader();
        Stream OpenFragmentShader();
    }
}