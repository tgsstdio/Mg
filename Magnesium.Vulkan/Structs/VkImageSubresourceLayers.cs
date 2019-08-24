using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
    [StructLayout(LayoutKind.Sequential)]
	internal struct VkImageSubresourceLayers
	{
		public MgImageAspectFlagBits aspectMask { get; set; }
		public UInt32 mipLevel { get; set; }
		public UInt32 baseArrayLayer { get; set; }
		public UInt32 layerCount { get; set; }
	}
}
