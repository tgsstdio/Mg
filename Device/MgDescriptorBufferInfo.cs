using System;

namespace Magnesium
{
    using VkBuffer = Magnesium.MgBuffer;

    // Device
    public class MgDescriptorBufferInfo
	{
		public VkBuffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Range { get; set; }
	}

}

