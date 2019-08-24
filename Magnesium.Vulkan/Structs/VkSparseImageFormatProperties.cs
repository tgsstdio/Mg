using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSparseImageFormatProperties
	{
		public MgImageAspectFlagBits aspectMask { get; set; }
		public MgExtent3D imageGranularity { get; set; }
		public MgSparseImageFormatFlagBits flags { get; set; }
	}
}
