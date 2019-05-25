using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkAttachmentReference2KHR
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt32 attachment;
		public MgImageLayout layout;
		public MgImageAspectFlagBits aspectMask;
	}
}
