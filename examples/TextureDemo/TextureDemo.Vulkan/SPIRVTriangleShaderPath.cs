using System;
using System.IO;
using TextureDemo.Core;

namespace TextureDemo.Vulkan
{
    public class VulkanDemoContent : ITextureDemoContent
    {           
        public Stream OpenFragmentShader()
        {
            return System.IO.File.OpenRead("Shaders/texture1.frag.spv");
        }

        public Stream OpenTextureFile()
        {
            return System.IO.File.OpenRead("Textures/pattern_02_bc2.ktx");
        }

        public Stream OpenVertexShader()
        {
            return System.IO.File.OpenRead("Shaders/texture1.vert.spv");
        }
    }
}
