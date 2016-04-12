using System;

namespace Magnesium
{
    public class MgImageMemoryBarrier
	{
		public MgAccessFlagBits SrcAccessMask { get; set; }
		public MgAccessFlagBits DstAccessMask { get; set; }
		public MgImageLayout OldLayout { get; set; }
		public MgImageLayout NewLayout { get; set; }
		public UInt32 SrcQueueFamilyIndex { get; set; }
		public UInt32 DstQueueFamilyIndex { get; set; }
		public MgImage Image { get; set; }
		public MgImageSubresourceRange SubresourceRange { get; set; }
	}
}

