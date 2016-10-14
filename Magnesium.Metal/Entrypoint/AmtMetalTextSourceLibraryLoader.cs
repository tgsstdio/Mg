using System;
using System.IO;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	public class AmtMetalTextSourceLibraryLoader : IAmtMetalLibraryLoader
	{
		public IMTLLibrary LoadLibrary(IMTLDevice device, MemoryStream ms)
		{
			using (var tr = new StreamReader(ms))
			{
				string source = tr.ReadToEnd();
				var options = new MTLCompileOptions
				{
					LanguageVersion = MTLLanguageVersion.v1_1,
					FastMathEnabled = false,
				};
				NSError err;
				IMTLLibrary library = device.CreateLibrary(source, options, out err);
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
