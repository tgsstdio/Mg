using System;

namespace Magnesium
{
    public class MgPipelineVertexInputStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgVertexInputBindingDescription[] VertexBindingDescriptions { get; set; }
		public MgVertexInputAttributeDescription[] VertexAttributeDescriptions { get; set; }
	}
}

