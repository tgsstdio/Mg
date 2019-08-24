using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkFormatProperties
	{
		// Format features in case of linear tiling
		public MgFormatFeatureFlagBits linearTilingFeatures;
		// Format features in case of optimal tiling
		public MgFormatFeatureFlagBits optimalTilingFeatures;
		// Format features supported by buffers
		public MgFormatFeatureFlagBits bufferFeatures;
	}
}
