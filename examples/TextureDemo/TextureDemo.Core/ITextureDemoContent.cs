using System.IO;

namespace TextureDemo.Core
{
    public interface ITextureDemoContent
    {
        Stream OpenVertexShader();
        Stream OpenFragmentShader();
        Stream OpenTextureFile();
    }
}