using System;

namespace Magnesium
{
    public class MgPipelineDepthStencilStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public bool DepthTestEnable { get; set; }
		public bool DepthWriteEnable { get; set; }
		public MgCompareOp DepthCompareOp { get; set; }
		public bool DepthBoundsTestEnable { get; set; }
		public bool StencilTestEnable { get; set; }
		public MgStencilOpState Front { get; set; }
		public MgStencilOpState Back { get; set; }
		public float MinDepthBounds { get; set; }
		public float MaxDepthBounds { get; set; }
	}
}

