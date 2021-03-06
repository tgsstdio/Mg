using System;

namespace Magnesium
{
    public class MgFramebufferCreateInfo
	{
		public UInt32 Flags { get; set; }
		public IMgRenderPass RenderPass { get; set; }
		public IMgImageView[] Attachments { get; set; }
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
		public UInt32 Layers { get; set; }
	}

}

