using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkImageMemoryBarrier
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkAccessFlags srcAccessMask { get; set; }
		public VkAccessFlags dstAccessMask { get; set; }
		public VkImageLayout oldLayout { get; set; }
		public VkImageLayout newLayout { get; set; }
		public UInt32 srcQueueFamilyIndex { get; set; }
		public UInt32 dstQueueFamilyIndex { get; set; }
		public UInt64 image { get; set; }
		public VkImageSubresourceRange subresourceRange { get; set; }
	}
}
