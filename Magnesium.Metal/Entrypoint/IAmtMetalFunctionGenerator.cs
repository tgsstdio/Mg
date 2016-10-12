using System;
using System.IO;
using Metal;

namespace Magnesium.Metal
{
	public interface IAmtMetalFunctionGenerator
	{
		IMTLFunction UsingSource(IMTLDevice device, MgPipelineShaderStageCreateInfo stage, MemoryStream ms);
	}
}
