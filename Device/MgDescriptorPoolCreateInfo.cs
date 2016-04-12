using System;

namespace Magnesium
{
    public class MgDescriptorPoolCreateInfo
	{
		public MgDescriptorPoolCreateFlagBits Flags { get; set; }
		public UInt32 MaxSets { get; set; }
		public MgDescriptorPoolSize[] PoolSizes { get; set; }
	}
}

