using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkRenderPassBeginInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 renderPass { get; set; }
		public UInt64 framebuffer { get; set; }
		public VkRect2D renderArea { get; set; }
		public UInt32 clearValueCount { get; set; }
		public IntPtr pClearValues { get; set; }
	}
}
