using System;
using System.IO;

namespace OffscreenDemo
{
    public interface IOffscreenDemoShaderPath
    {
        Stream OpenVertexShader();
        Stream OpenFragmentShader();
    }
}