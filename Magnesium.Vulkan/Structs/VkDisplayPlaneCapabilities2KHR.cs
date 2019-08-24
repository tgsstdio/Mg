using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
    [StructLayout(LayoutKind.Sequential)]
	internal struct VkDisplayPlaneCapabilities2KHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkDisplayPlaneCapabilitiesKHR capabilities { get; set; }
	}
}
