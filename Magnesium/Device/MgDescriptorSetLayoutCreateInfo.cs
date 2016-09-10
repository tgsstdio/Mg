using System;

namespace Magnesium
{
    public class MgDescriptorSetLayoutCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgDescriptorSetLayoutBinding[] Bindings { get; set; }
	}
}

