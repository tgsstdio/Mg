using Magnesium;
using System.IO;

namespace MonoGame.Textures.Ktx
{
    public interface IKTXTextureLoader
    {
        MgTextureInfo[] Load(Stream fs);
    }
}