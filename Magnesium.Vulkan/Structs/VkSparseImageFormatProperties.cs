using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSparseImageFormatProperties
	{
		public VkImageAspectFlags aspectMask { get; set; }
		public MgExtent3D imageGranularity { get; set; }
		public VkSparseImageFormatFlags flags { get; set; }
	}
}
