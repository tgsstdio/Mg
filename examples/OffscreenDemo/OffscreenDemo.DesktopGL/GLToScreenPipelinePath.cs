using System.IO;

namespace OffscreenDemo.DesktopGL
{
    internal class GLToScreenPipelinePath : IToScreenPipelineMediaPath
    {
        public Stream OpenFragmentShader()
        {
            return File.Open("Shaders/texture1.frag", FileMode.Open);
        }

        public Stream OpenVertexShader()
        {
            return File.Open("Shaders/texture1.vert", FileMode.Open);
        }
    }
}