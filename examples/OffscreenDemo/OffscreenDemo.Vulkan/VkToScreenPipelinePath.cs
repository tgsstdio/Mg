using System.IO;

namespace OffscreenDemo
{
    internal class VkToScreenPipelinePath : IToScreenPipelineMediaPath
    {
        public Stream OpenFragmentShader()
        {
            return File.Open("Shaders/texture1.frag.spv", FileMode.Open);
        }

        public Stream OpenVertexShader()
        {
            return File.Open("Shaders/texture1.vert.spv", FileMode.Open);
        }
    }
}