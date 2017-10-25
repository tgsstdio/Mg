using System.IO;

namespace OffscreenDemo.DesktopGL
{
    internal class GLOffscreenDemoShaderPath : IOffscreenPipelineMediaPath
    {
        public Stream OpenFragmentShader()
        {
            return File.Open("Shaders/triangle.frag", FileMode.Open);
        }

        public Stream OpenVertexShader()
        {
            return File.Open("Shaders/triangle.vert", FileMode.Open);
        }
    }
}