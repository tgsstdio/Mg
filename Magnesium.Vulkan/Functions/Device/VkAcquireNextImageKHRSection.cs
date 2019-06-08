using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkAcquireNextImageKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkAcquireNextImageKHR(IntPtr device, UInt64 swapchain, UInt64 timeout, UInt64 semaphore, UInt64 fence, ref UInt32 pImageIndex);

		public static MgResult AcquireNextImageKHR(VkDeviceInfo info, IMgSwapchainKHR swapchain, UInt64 timeout, IMgSemaphore semaphore, IMgFence fence, out UInt32 pImageIndex)
		{
			// TODO: add implementation
		}
	}
}
