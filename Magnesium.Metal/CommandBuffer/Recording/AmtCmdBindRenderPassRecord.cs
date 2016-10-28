using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtCmdBindRenderPassRecord
	{
		public MTLRenderPassDescriptor Descriptor { get; set; }
		public IAmtImageView[] ColorAttachments { get; set;}
		public IAmtImageView DepthStencil { get; set; }
	}
}
