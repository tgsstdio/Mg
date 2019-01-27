using System;

namespace Magnesium
{
	public class MgRenderPassCreateInfo2KHR
	{
		public UInt32 Flags { get; set; }
		public MgAttachmentDescription2KHR[] Attachments { get; set; }
		public MgSubpassDescription2KHR[] Subpasses { get; set; }
		public MgSubpassDependency2KHR Dependencies { get; set; }
		public UInt32[] CorrelatedViewMasks { get; set; }
	}
}
