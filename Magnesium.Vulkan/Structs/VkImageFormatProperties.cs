using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkImageFormatProperties
	{
		public MgExtent3D maxExtent { get; set; }
		public UInt32 maxMipLevels { get; set; }
		public UInt32 maxArrayLayers { get; set; }
		public VkSampleCountFlags sampleCounts { get; set; }
		public UInt64 maxResourceSize { get; set; }
	}
}
