using System;
using System.IO;

namespace OffscreenDemo
{
    public interface IPostProcessingPassThruMediaPath
    {
        Stream OpenVertexShader();
        Stream OpenFragmentShader();
    }
}