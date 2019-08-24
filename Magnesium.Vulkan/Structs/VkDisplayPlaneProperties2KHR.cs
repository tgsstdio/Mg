using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDisplayPlaneProperties2KHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkDisplayPlanePropertiesKHR displayPlaneProperties { get; set; }
	}
}
