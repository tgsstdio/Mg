using Magnesium;
using System.IO;

namespace Magnesium.Mtx
{
    public interface IKTXTextureLoader
    {
        KTXTextureOutput Load(Stream fs);
    }
}