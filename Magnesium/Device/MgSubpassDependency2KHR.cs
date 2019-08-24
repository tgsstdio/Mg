using System;

namespace Magnesium
{
	public class MgSubpassDependency2KHR
	{
		public UInt32 SrcSubpass { get; set; }
		public UInt32 DstSubpass { get; set; }
		public MgPipelineStageFlagBits SrcStageMask { get; set; }
		public MgPipelineStageFlagBits DstStageMask { get; set; }
		public MgAccessFlagBits SrcAccessMask { get; set; }
		public MgAccessFlagBits DstAccessMask { get; set; }
		public MgDependencyFlagBits DependencyFlags { get; set; }
		public Int32 ViewOffset { get; set; }
	}
}
