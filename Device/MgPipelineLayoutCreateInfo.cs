using System;

namespace Magnesium
{
    public class MgPipelineLayoutCreateInfo
	{
		public UInt32 Flags { get; set; }
		public IMgDescriptorSetLayout[] SetLayouts { get; set; }
		public MgPushConstantRange[] PushConstantRanges { get; set; }
	}
}

