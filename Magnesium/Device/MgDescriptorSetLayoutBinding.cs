using System;

namespace Magnesium
{
    public class MgDescriptorSetLayoutBinding
	{
		public UInt32 Binding { get; set; }
		public MgDescriptorType DescriptorType { get; set; }
		public UInt32 DescriptorCount { get; set; }
		public MgShaderStageFlagBits StageFlags { get; set; }
		public IMgSampler[] ImmutableSamplers { get; set; }
	}
}

