using System;
using System.IO;

namespace TriangleDemo.DesktopGL
{
    internal class GLSLTriangleShaderPath : ITriangleDemoShaderPath
    {
        public Stream OpenFragmentShader()
        {
            throw new NotImplementedException();
        }

        public Stream OpenVertexShader()
        {
            throw new NotImplementedException();
        }
    }
}