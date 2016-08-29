using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDisplayPlanePropertiesKHR
	{
		public UInt64 currentDisplay { get; set; }
		public UInt32 currentStackIndex { get; set; }
	}
}
