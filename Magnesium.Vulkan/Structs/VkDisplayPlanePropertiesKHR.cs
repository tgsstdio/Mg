using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDisplayPlanePropertiesKHR
	{
		public UInt64 currentDisplay { get; set; }
		public UInt32 currentStackIndex { get; set; }
	}
}
