using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkAttachmentDescription
	{
		public VkAttachmentDescriptionFlags flags { get; set; }
		public VkFormat format { get; set; }
		public VkSampleCountFlags samples { get; set; }
		public VkAttachmentLoadOp loadOp { get; set; }
		public VkAttachmentStoreOp storeOp { get; set; }
		public VkAttachmentLoadOp stencilLoadOp { get; set; }
		public VkAttachmentStoreOp stencilStoreOp { get; set; }
		public VkImageLayout initialLayout { get; set; }
		public VkImageLayout finalLayout { get; set; }
	}
}
