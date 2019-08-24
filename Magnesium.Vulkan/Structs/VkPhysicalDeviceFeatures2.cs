using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceFeatures2
	{
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPhysicalDeviceFeatures features;
	}
}
