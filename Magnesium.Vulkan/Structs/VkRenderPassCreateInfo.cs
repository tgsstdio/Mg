using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkRenderPassCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt32 attachmentCount { get; set; }
		public IntPtr pAttachments { get; set; }
		public UInt32 subpassCount { get; set; }
		public IntPtr pSubpasses { get; set; }
		public UInt32 dependencyCount { get; set; }
		public IntPtr pDependencies { get; set; }
	}
}
