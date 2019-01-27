using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceProperties2
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public VkPhysicalDeviceProperties properties;
	}
}
