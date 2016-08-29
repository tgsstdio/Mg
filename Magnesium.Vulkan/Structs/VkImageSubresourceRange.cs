using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkImageSubresourceRange
	{
		public VkImageAspectFlags aspectMask { get; set; }
		public UInt32 baseMipLevel { get; set; }
		public UInt32 levelCount { get; set; }
		public UInt32 baseArrayLayer { get; set; }
		public UInt32 layerCount { get; set; }
	}
}
