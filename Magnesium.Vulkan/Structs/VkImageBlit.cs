using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkImageBlit
	{
		public VkImageSubresourceLayers srcSubresource { get; set; }
		public MgOffset3D srcOffsets { get; set; }
		public VkImageSubresourceLayers dstSubresource { get; set; }
		public MgOffset3D dstOffsets { get; set; }
	}
}
