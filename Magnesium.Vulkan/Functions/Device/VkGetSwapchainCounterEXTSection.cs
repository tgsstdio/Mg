using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetSwapchainCounterEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetSwapchainCounterEXT(IntPtr device, UInt64 swapchain, MgSurfaceCounterFlagBitsEXT counter, ref UInt64 pCounterValue);

		public static MgResult GetSwapchainCounterEXT(VkDeviceInfo info, IMgSwapchainKHR swapchain, MgSurfaceCounterFlagBitsEXT counter, ref UInt64 pCounterValue)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
