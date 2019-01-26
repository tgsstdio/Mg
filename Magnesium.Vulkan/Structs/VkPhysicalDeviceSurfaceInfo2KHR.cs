using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceSurfaceInfo2KHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 surface { get; set; }
	}
}
