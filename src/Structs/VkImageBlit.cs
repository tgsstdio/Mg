using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkImageBlit
	{
		public VkImageSubresourceLayers srcSubresource { get; set; }
		public VkOffset3D srcOffsets { get; set; }
		public VkImageSubresourceLayers dstSubresource { get; set; }
		public VkOffset3D dstOffsets { get; set; }
	}
}
