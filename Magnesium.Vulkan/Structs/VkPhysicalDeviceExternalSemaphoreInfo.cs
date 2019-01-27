using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceExternalSemaphoreInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgExternalSemaphoreHandleTypeFlagBits handleType { get; set; }
	}
}
