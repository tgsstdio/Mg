using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkAttachmentDescription2KHR
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public MgAttachmentDescriptionFlagBits flags;
		public MgFormat format;
		public MgSampleCountFlagBits samples;
		// Load operation for color or depth data
		public VkAttachmentLoadOp loadOp;
		// Store operation for color or depth data
		public VkAttachmentStoreOp storeOp;
		// Load operation for stencil data
		public VkAttachmentLoadOp stencilLoadOp;
		// Store operation for stencil data
		public VkAttachmentStoreOp stencilStoreOp;
		public MgImageLayout initialLayout;
		public MgImageLayout finalLayout;
	}
}
