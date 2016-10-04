using System;
namespace Magnesium.Metal
{
	public class AmtFramebufferSubpassInfo
	{
		public AmtImageView[] ColorAttachments { get; set; }
		public AmtImageView DepthStencil { get; set;}
	}
}
