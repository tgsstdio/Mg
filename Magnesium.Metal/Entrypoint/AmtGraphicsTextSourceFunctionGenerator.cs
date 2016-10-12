using System;
using System.IO;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsTextSourceFunctionGenerator : IAmtGraphicsFunctionGenerator
	{
		public IMTLFunction UsingSource(IMTLDevice device, MgPipelineShaderStageCreateInfo stage, MemoryStream ms)
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
				IMTLFunction shaderFunc = library.CreateFunction(stage.Name);
				return shaderFunc;
			}
		}
	}
}
