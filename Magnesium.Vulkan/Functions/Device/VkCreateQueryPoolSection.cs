using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateQueryPoolSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateQueryPool(IntPtr device, [In, Out] VkQueryPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pQueryPool);

		public static MgResult CreateQueryPool(VkDeviceInfo info, MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool)
		{
			// TODO: add implementation
		}
	}
}
