using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkImageViewCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt64 image { get; set; }
		public VkImageViewType viewType { get; set; }
		public VkFormat format { get; set; }
		public VkComponentMapping components { get; set; }
		public VkImageSubresourceRange subresourceRange { get; set; }
	}
}
