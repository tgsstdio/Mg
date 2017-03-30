using System;
using System.IO;
using TextureDemo.Core;

namespace TextureDemo.DesktopGL
{
    internal class DesktopGLContent : ITextureDemoContent
    {
        public Stream OpenFragmentShader()
        {
            return System.IO.File.OpenRead("Shaders/textureGL.frag");
        }

        public Stream OpenTextureFile()
        {
            return System.IO.File.OpenRead("Textures/pattern_02_bc2.ktx");
        }

        public Stream OpenVertexShader()
        {
            return System.IO.File.OpenRead("Shaders/textureGL.vert");
        }
    }
}