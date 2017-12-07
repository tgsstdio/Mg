using System.IO;

namespace OffscreenDemo.DesktopGL
{
    class GLRenderToTexturePath : IRenderToTexturePipelineMediaPath
    {
        public Stream OpenFragmentShader()
        {
           return File.Open("Shaders/RenderToTexture.vert", FileMode.Open);
        }

        public Stream OpenVertexShader()
        {
            return File.Open("Shaders/RenderToTexture.vert", FileMode.Open);
        }
    }
}