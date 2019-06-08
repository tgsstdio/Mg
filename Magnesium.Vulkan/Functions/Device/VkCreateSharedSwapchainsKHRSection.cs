using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateSharedSwapchainsKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateSharedSwapchainsKHR(IntPtr device, UInt32 swapchainCount, [In, Out] VkSwapchainCreateInfoKHR[] pCreateInfos, IntPtr pAllocator, UInt64[] pSwapchains);

		public static MgResult CreateSharedSwapchainsKHR(VkDeviceInfo info, MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
		{
			// TODO: add implementation
		}
	}
}
