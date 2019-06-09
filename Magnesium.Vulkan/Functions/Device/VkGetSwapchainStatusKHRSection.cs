using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetSwapchainStatusKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetSwapchainStatusKHR(IntPtr device, UInt64 swapchain);

		public static MgResult GetSwapchainStatusKHR(VkDeviceInfo info, IMgSwapchainKHR swapchain)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
