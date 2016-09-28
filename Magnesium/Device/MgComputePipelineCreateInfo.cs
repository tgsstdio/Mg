using System;

namespace Magnesium
{
    public class MgComputePipelineCreateInfo
	{
		public MgPipelineCreateFlagBits Flags { get; set; }
		public MgPipelineShaderStageCreateInfo Stage { get; set; }
		public IMgPipelineLayout Layout { get; set; }
		// REQUIRED FOR METAL
		public MgVec3Ui ThreadsPerWorkgroup { get; set;}
		public IMgPipeline BasePipelineHandle { get; set; }
		public Int32 BasePipelineIndex { get; set; }
	}
}

