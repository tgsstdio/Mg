using System;

namespace Magnesium
{
    public class MgCommandBufferAllocateInfo
	{
		public IMgCommandPool CommandPool { get; set; }
		public MgCommandBufferLevel Level { get; set; }
		public UInt32 CommandBufferCount { get; set; }
	}

}

