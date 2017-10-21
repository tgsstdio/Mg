using System;
using System.IO;

namespace OffscreenDemo
{
    public interface IOffscreenPipelineMediaPath
    {
        Stream OpenVertexShader();
        Stream OpenFragmentShader();
    }
}