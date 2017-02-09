using System;
using System.IO;

namespace TriangleDemo.Vulkan
{
    public class SPIRVTriangleShaderPath : ITriangleDemoShaderPath
    {           
        public Stream OpenFragmentShader()
        {
            return System.IO.File.OpenRead("shaders/triangle.frag.spv");
        }

        public Stream OpenVertexShader()
        {
            return System.IO.File.OpenRead("shaders/triangle.vert.spv");
        }
    }
}
