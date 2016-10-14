using System.IO;
using Metal;

namespace Magnesium.Metal
{
	public interface IAmtMetalLibraryLoader
	{
		IMTLLibrary LoadLibrary(IMTLDevice device, MemoryStream ms);
	}
}
