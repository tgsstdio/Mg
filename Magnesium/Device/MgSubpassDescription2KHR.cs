using System;

namespace Magnesium
{
	public class MgSubpassDescription2KHR
	{
		public MgSubpassDescriptionFlagBits Flags { get; set; }
		public MgPipelineBindPoint PipelineBindPoint { get; set; }
		public UInt32 ViewMask { get; set; }
		//public UInt32 InputAttachmentCount { get; set; }
		public MgAttachmentReference2KHR[] InputAttachments { get; set; }
		//public UInt32 ColorAttachmentCount { get; set; }
		public MgAttachmentReference2KHR[] ColorAttachments { get; set; }
		public MgAttachmentReference2KHR[] ResolveAttachments { get; set; }
		public MgAttachmentReference2KHR[] DepthStencilAttachment { get; set; }
		//public UInt32 PreserveAttachmentCount { get; set; }
		public UInt32[] PreserveAttachments { get; set; }
	}
}
