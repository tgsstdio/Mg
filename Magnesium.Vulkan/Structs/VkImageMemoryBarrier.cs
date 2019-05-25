using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkImageMemoryBarrier
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgAccessFlagBits srcAccessMask { get; set; }
		public MgAccessFlagBits dstAccessMask { get; set; }
		public MgImageLayout oldLayout { get; set; }
		public MgImageLayout newLayout { get; set; }
		public UInt32 srcQueueFamilyIndex { get; set; }
		public UInt32 dstQueueFamilyIndex { get; set; }
		public UInt64 image { get; set; }
		public VkImageSubresourceRange subresourceRange { get; set; }
	}
}
