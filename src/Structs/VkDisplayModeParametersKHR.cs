using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDisplayModeParametersKHR
	{
		public MgExtent2D visibleRegion { get; set; }
		public UInt32 refreshRate { get; set; }
	}
}
