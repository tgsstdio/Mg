using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateSwapchainKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateSwapchainKHR(IntPtr device, [In, Out] VkSwapchainCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSwapchain);

		public static MgResult CreateSwapchainKHR(VkDeviceInfo info, MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
		{
			// TODO: add implementation
		}
	}
}
