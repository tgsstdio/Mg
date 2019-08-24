using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSurfaceFormat2KHR
	{
        public VkStructureType sType;
        public IntPtr pNext;
        public VkSurfaceFormatKHR surfaceFormat;
	}
}
