using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSurfaceCapabilities2KHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkSurfaceCapabilitiesKHR surfaceCapabilities { get; set; }
	}
}
