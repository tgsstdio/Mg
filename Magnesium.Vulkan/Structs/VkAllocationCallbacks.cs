using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkAllocationCallbacks
	{
		public IntPtr pUserData { get; set; }
		public IntPtr pfnAllocation { get; set; } // PFN_vkAllocationFunction
		public IntPtr pfnReallocation { get; set; } // PFN_vkReallocationFunction
		public IntPtr pfnFree { get; set; } // PFN_vkFreeFunction
		public IntPtr pfnInternalAllocation { get; set; } // PFN_vkInternalAllocationNotification
		public IntPtr pfnInternalFree { get; set; } // PFN_vkInternalFreeNotification
}
}
