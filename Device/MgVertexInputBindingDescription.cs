using System;

namespace Magnesium
{
    public class MgVertexInputBindingDescription
	{
		public UInt32 Binding { get; set; }
		public UInt32 Stride { get; set; }
		public MgVertexInputRate InputRate { get; set; }
	}
}

