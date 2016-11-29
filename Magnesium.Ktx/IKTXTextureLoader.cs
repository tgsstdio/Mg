using Magnesium;
using System.IO;

namespace Magnesium.Ktx
{
    public interface IKTXTextureLoader
    {
        MgTextureInfo[] Load(Stream fs);
    }
}