using System;
using System.IO;

namespace TriangleDemo.DesktopGL
{
    internal class GLSLTriangleShaderPath : ITriangleDemoShaderPath
    {
        public Stream OpenFragmentShader()
        {
            return System.IO.File.OpenRead("shaders/triangle.frag");
        }

        public Stream OpenVertexShader()
        {
            return System.IO.File.OpenRead("shaders/triangle3.vert");
        }
    }
}