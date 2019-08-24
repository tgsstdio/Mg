using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkAcquireNextImageKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkAcquireNextImageKHR(IntPtr device, UInt64 swapchain, UInt64 timeout, UInt64 semaphore, UInt64 fence, ref UInt32 pImageIndex);

        public static MgResult AcquireNextImageKHR(VkDeviceInfo info, IMgSwapchainKHR swapchain, UInt64 timeout, IMgSemaphore semaphore, IMgFence fence, out UInt32 pImageIndex)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bSwapchain = (VkSwapchainKHR)swapchain;
            Debug.Assert(bSwapchain != null);

            var bSemaphore = (VkSemaphore)semaphore;
            var bSemaphorePtr = bSemaphore != null ? bSemaphore.Handle : 0UL;

            var bFence = (VkFence)fence;
            var bFencePtr = bFence != null ? bFence.Handle : 0UL;

            uint imageIndex = 0;
            var result = vkAcquireNextImageKHR(info.Handle, bSwapchain.Handle, timeout, bSemaphorePtr, bFencePtr, ref imageIndex);
            pImageIndex = imageIndex;
            return result;
        }
    }
}
