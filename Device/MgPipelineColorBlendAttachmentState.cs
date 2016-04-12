using System;

namespace Magnesium
{
    public class MgPipelineColorBlendAttachmentState
	{
		public bool BlendEnable { get; set; }
		public MgBlendFactor SrcColorBlendFactor { get; set; }
		public MgBlendFactor DstColorBlendFactor { get; set; }
		public MgBlendOp ColorBlendOp { get; set; }
		public MgBlendFactor SrcAlphaBlendFactor { get; set; }
		public MgBlendFactor DstAlphaBlendFactor { get; set; }
		public MgBlendOp AlphaBlendOp { get; set; }
		public UInt32 ColorWriteMask { get; set; }
	}
}

