using System;

namespace Magnesium
{
    public class MgPipelineColorBlendStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public bool LogicOpEnable { get; set; }
		public MgLogicOp LogicOp { get; set; }
		public MgPipelineColorBlendAttachmentState[] Attachments { get; set; }
		public MgVec4f BlendConstants { get; set; } // 4
	}
}

