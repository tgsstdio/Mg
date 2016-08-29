using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkFramebufferCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt64 renderPass { get; set; }
		public UInt32 attachmentCount { get; set; }
		public IntPtr pAttachments { get; set; }
		public UInt32 width { get; set; }
		public UInt32 height { get; set; }
		public UInt32 layers { get; set; }
	}
}
