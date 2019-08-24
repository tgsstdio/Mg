using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSubpassDescription2KHR
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public MgSubpassDescriptionFlagBits flags;
		public MgPipelineBindPoint pipelineBindPoint;
		public UInt32 viewMask;
		public UInt32 inputAttachmentCount;
		public IntPtr pInputAttachments;
		public UInt32 colorAttachmentCount;
		public IntPtr pColorAttachments;
		public IntPtr pResolveAttachments;
		public IntPtr pDepthStencilAttachment;
		public UInt32 preserveAttachmentCount;
		public IntPtr pPreserveAttachments;
	}
}
