using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtCmdBindDescriptorSetBufferRecord
	{
		public IMTLBuffer Buffer { get; set; }
		public nuint Offset { get; set; }
	}
}