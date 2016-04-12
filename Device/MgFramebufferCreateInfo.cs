using System;

namespace Magnesium
{
    public class MgFramebufferCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgRenderPass RenderPass { get; set; }
		public MgImageView[] Attachments { get; set; }
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
		public UInt32 Layers { get; set; }
	}

}

