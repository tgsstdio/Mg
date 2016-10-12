using System;
using System.IO;
using Metal;

namespace Magnesium.Metal
{
	public interface IAmtGraphicsFunctionGenerator
	{
		IMTLFunction UsingSource(IMTLDevice device, MgPipelineShaderStageCreateInfo stage, MemoryStream ms);
	}
}
