using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkAttachmentDescription
	{
		public MgAttachmentDescriptionFlagBits flags { get; set; }
		public MgFormat format { get; set; }
		public MgSampleCountFlagBits samples { get; set; }
		public VkAttachmentLoadOp loadOp { get; set; }
		public VkAttachmentStoreOp storeOp { get; set; }
		public VkAttachmentLoadOp stencilLoadOp { get; set; }
		public VkAttachmentStoreOp stencilStoreOp { get; set; }
		public MgImageLayout initialLayout { get; set; }
		public MgImageLayout finalLayout { get; set; }
	}
}
