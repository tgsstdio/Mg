using System;
namespace Magnesium.Metal
{
	public class AmtFramebufferSubpassInfo
	{
		public IAmtImageView[] ColorAttachments { get; set; }
		public IAmtImageView DepthStencil { get; set;}
	}
}
