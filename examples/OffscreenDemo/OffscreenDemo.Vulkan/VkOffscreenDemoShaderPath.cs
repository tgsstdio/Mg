using System.IO;

namespace OffscreenDemo
{
    internal class VkOffscreenDemoShaderPath : IOffscreenPipelineMediaPath
    {
        public Stream OpenFragmentShader()
        {
            return File.Open("Shaders/triangle.frag.spv", FileMode.Open);
        }

        public Stream OpenVertexShader()
        {
            return File.Open("Shaders/triangle.vert.spv", FileMode.Open);
        }
    }
}