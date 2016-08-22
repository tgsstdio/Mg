using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkImageSubresource
	{
		public VkImageAspectFlags aspectMask { get; set; }
		public UInt32 mipLevel { get; set; }
		public UInt32 arrayLayer { get; set; }
	}
}
