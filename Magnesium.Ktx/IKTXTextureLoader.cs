using Magnesium;
using System.IO;

namespace Magnesium.Ktx
{
    public interface IKTXTextureLoader
    {
        KTXTextureOutput Load(Stream fs);
    }
}