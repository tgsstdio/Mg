using System.IO;

namespace OffscreenDemo.DesktopGL
{
    class GLPostProcessPassThruMediaPath : IPostProcessingPassThruMediaPath
    {
        public Stream OpenFragmentShader()
        {
            return File.Open("Shaders/PostProcessingPassThru.frag", FileMode.Open);
        }

        public Stream OpenVertexShader()
        {
            return File.Open("Shaders/PostProcessingPassThru.vert", FileMode.Open);
        }
    }
}