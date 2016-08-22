using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkAllocationCallbacks
	{
		public IntPtr pUserData { get; set; }
		public PFN_vkAllocationFunction pfnAllocation { get; set; }
		public PFN_vkReallocationFunction pfnReallocation { get; set; }
		public PFN_vkFreeFunction pfnFree { get; set; }
		public PFN_vkInternalAllocationNotification pfnInternalAllocation { get; set; }
		public PFN_vkInternalFreeNotification pfnInternalFree { get; set; }
	}
}
