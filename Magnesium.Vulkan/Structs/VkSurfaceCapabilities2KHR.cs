using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSurfaceCapabilities2KHR
	{
        public VkStructureType sType;
        public IntPtr pNext;
        public VkSurfaceCapabilitiesKHR surfaceCapabilities;
	}
}
