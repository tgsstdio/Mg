using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetSwapchainCounterEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetSwapchainCounterEXT(IntPtr device, UInt64 swapchain, VkSurfaceCounterFlagBitsEXT counter, ref UInt64 pCounterValue);

		public static MgResult GetSwapchainCounterEXT(VkDeviceInfo info, IMgSwapchainKHR swapchain, MgSurfaceCounterFlagBitsEXT counter, ref UInt64 pCounterValue)
		{
			// TODO: add implementation
		}
	}
}
