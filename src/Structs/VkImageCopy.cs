using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkImageCopy
	{
		public VkImageSubresourceLayers srcSubresource { get; set; }
		public VkOffset3D srcOffset { get; set; }
		public VkImageSubresourceLayers dstSubresource { get; set; }
		public VkOffset3D dstOffset { get; set; }
		public VkExtent3D extent { get; set; }
	}
}
