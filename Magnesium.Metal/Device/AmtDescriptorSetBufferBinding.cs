using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDescriptorSetBufferBinding
	{
		public IMTLBuffer Buffer { get; set;}
		public nuint BoundMemoryOffset { get; internal set; }
	}
}