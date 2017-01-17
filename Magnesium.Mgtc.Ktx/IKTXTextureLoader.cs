using Magnesium;
using System.IO;

namespace Mgtc
{
    public interface IKTXTextureLoader
    {
        KTXTextureOutput Load(Stream fs);
    }
}