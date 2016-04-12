using System;

namespace Magnesium
{
    public class MgBufferMemoryBarrier
	{
		public MgAccessFlagBits SrcAccessMask { get; set; }
		public MgAccessFlagBits DstAccessMask { get; set; }
		public UInt32 SrcQueueFamilyIndex { get; set; }
		public UInt32 DstQueueFamilyIndex { get; set; }
		public MgBuffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
	}
}

