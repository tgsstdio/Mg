using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkClearAttachment
	{
		public VkImageAspectFlags aspectMask { get; set; }
		public UInt32 colorAttachment { get; set; }
		public VkClearValue clearValue { get; set; }
	}
}
