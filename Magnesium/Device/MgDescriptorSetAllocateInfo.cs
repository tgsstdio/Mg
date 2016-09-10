using System;

namespace Magnesium
{
    // Device
    public class MgDescriptorSetAllocateInfo
	{
		public UInt32 DescriptorSetCount { get; set; }
		public IMgDescriptorPool DescriptorPool { get; set; }
		public IMgDescriptorSetLayout[] SetLayouts { get; set; }
	}
}

