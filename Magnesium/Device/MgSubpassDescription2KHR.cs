using System;

namespace Magnesium
{
	public class MgSubpassDescription2KHR
	{
		public VkSubpassDescriptionFlags Flags { get; set; }
		public VkPipelineBindPoint PipelineBindPoint { get; set; }
		public UInt32 ViewMask { get; set; }
		public UInt32 InputAttachmentCount { get; set; }
		public VkAttachmentReference2KHR PInputAttachments { get; set; }
		public UInt32 ColorAttachmentCount { get; set; }
		public VkAttachmentReference2KHR PColorAttachments { get; set; }
		public VkAttachmentReference2KHR PResolveAttachments { get; set; }
		public VkAttachmentReference2KHR PDepthStencilAttachment { get; set; }
		public UInt32 PreserveAttachmentCount { get; set; }
		public UInt32 PPreserveAttachments { get; set; }
	}
}
