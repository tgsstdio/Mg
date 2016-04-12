using System;

namespace Magnesium
{
    public class MgCopyDescriptorSet
	{
		public MgDescriptorSet SrcSet { get; set; }
		public UInt32 SrcBinding { get; set; }
		public UInt32 SrcArrayElement { get; set; }
		public MgDescriptorSet DstSet { get; set; }
		public UInt32 DstBinding { get; set; }
		public UInt32 DstArrayElement { get; set; }
		public UInt32 DescriptorCount { get; set; }
	}
}

