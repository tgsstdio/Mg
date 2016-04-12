using System;

namespace Magnesium
{
    public class MgPipelineShaderStageCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgShaderStageFlagBits Stage { get; set; }
		public MgShaderModule Module { get; set; }
		public String Name { get; set; }
		public MgSpecializationInfo SpecializationInfo { get; set; }
	}
}

