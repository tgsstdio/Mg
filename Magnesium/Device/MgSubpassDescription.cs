using System;

namespace Magnesium
{
    public class MgSubpassDescription
	{
		public UInt32 Flags { get; set; }
		public MgPipelineBindPoint PipelineBindPoint { get; set; }
		public MgAttachmentReference[] InputAttachments { get; set; }
		public MgAttachmentReference[] ColorAttachments { get; set; }
		public MgAttachmentReference[] ResolveAttachments { get; set; }
		public MgAttachmentReference DepthStencilAttachment { get; set; }
		public UInt32[] PreserveAttachments { get; set; }
	}
}

