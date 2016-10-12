using System;
using System.IO;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	// FIXME: does not work ATM
	public class AmtGraphicsByteCodeFunctionGenerator : IAmtGraphicsFunctionGenerator 
	{
		public IMTLFunction UsingSource(IMTLDevice device, MgPipelineShaderStageCreateInfo stage, MemoryStream ms)
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
				IMTLFunction shaderFunc = library.CreateFunction(stage.Name);
				return shaderFunc;
			}
		}
	}
}
