using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceMemoryProperties2
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public VkPhysicalDeviceMemoryProperties memoryProperties;
	}
}
