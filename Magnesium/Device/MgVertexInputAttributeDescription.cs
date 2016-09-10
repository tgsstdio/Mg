using System;

namespace Magnesium
{
    public class MgVertexInputAttributeDescription
	{
		public UInt32 Location { get; set; }
		public UInt32 Binding { get; set; }
		public MgFormat Format { get; set; }
		public UInt32 Offset { get; set; }
	}
}

