using System;
using System.IO;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	// FIXME: does not work ATM
	public class AmtMetalByteCodeLibraryLoader : IAmtMetalLibraryLoader 
	{
		public IMTLLibrary LoadLibrary(IMTLDevice device, MemoryStream ms)
		{
			// UPDATE SHADERMODULE wIth FUNCTION FOR REUSE
			var byteArray = ms.ToArray();

			using (NSData data = NSData.FromArray(byteArray))
			{
				NSError err;
				IMTLLibrary library = device.CreateLibrary(data, out err);
				if (library == null)
				{
					// TODO: better error handling
					throw new Exception(err.ToString());
				}
				return library;
			}
		}
	}
}
