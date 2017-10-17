using System;
using System.IO;

namespace OffscreenDemo
{
    public interface IToScreenPipelinePath
    {
        Stream OpenVertexShader();
        Stream OpenFragmentShader();
    }
}