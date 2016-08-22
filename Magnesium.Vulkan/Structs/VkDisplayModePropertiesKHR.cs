using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDisplayModePropertiesKHR
	{
		public UInt64 displayMode { get; set; }
		public VkDisplayModeParametersKHR parameters { get; set; }
	}
}
