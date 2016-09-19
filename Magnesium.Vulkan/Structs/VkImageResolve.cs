using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkImageResolve
	{
		public VkImageSubresourceLayers srcSubresource { get; set; }
		public MgOffset3D srcOffset { get; set; }
		public VkImageSubresourceLayers dstSubresource { get; set; }
		public MgOffset3D dstOffset { get; set; }
		public MgExtent3D extent { get; set; }
	}
}
