using Magnesium;
using System.IO;

namespace Magnesium.Mtx
{
    public interface IMtxTextureLoader
    {
        KTXTextureOutput Load(Stream fs);
    }
}