using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkFormatProperties
	{
		public VkFormatFeatureFlags linearTilingFeatures { get; set; }
		public VkFormatFeatureFlags optimalTilingFeatures { get; set; }
		public VkFormatFeatureFlags bufferFeatures { get; set; }
	}
}
