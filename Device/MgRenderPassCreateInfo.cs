using System;

namespace Magnesium
{
    public class MgRenderPassCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgAttachmentDescription[] Attachments { get; set; }
		public MgSubpassDescription[] Subpasses { get; set; }
		public MgSubpassDependency[] Dependencies { get; set; }
	}
}

