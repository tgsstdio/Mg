using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSubpassDescription
	{
		public UInt32 flags { get; set; }
		public VkPipelineBindPoint pipelineBindPoint { get; set; }
		public UInt32 inputAttachmentCount { get; set; }
		public VkAttachmentReference pInputAttachments { get; set; }
		public UInt32 colorAttachmentCount { get; set; }
		public VkAttachmentReference pColorAttachments { get; set; }
		public VkAttachmentReference pResolveAttachments { get; set; }
		public VkAttachmentReference pDepthStencilAttachment { get; set; }
		public UInt32 preserveAttachmentCount { get; set; }
		public UInt32 pPreserveAttachments { get; set; }
	}
}
