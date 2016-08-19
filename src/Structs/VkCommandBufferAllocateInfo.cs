using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkCommandBufferAllocateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 commandPool { get; set; }
		public VkCommandBufferLevel level { get; set; }
		public UInt32 commandBufferCount { get; set; }
	}
}
