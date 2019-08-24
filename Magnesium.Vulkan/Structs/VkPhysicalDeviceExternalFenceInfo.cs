using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceExternalFenceInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgExternalFenceHandleTypeFlagBits handleType { get; set; }
	}
}
