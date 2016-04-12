using System;

namespace Magnesium
{
    public class MgComputePipelineCreateInfo
	{
		public MgPipelineCreateFlagBits Flags { get; set; }
		public MgPipelineShaderStageCreateInfo Stage { get; set; }
		public MgPipelineLayout Layout { get; set; }
		public MgPipeline BasePipelineHandle { get; set; }
		public Int32 BasePipelineIndex { get; set; }
	}
}

