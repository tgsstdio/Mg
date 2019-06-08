using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetSwapchainImagesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetSwapchainImagesKHR(IntPtr device, UInt64 swapchain, ref UInt32 pSwapchainImageCount, UInt64[] pSwapchainImages);

		public static MgResult GetSwapchainImagesKHR(VkDeviceInfo info, IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
		{
			// TODO: add implementation
		}
	}
}
