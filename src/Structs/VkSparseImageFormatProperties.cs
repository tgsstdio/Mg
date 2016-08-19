using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSparseImageFormatProperties
	{
		public VkImageAspectFlags aspectMask { get; set; }
		public VkExtent3D imageGranularity { get; set; }
		public VkSparseImageFormatFlags flags { get; set; }
	}
}
