using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkAcquireNextImage2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkAcquireNextImage2KHR(IntPtr device, [In, Out] VkAcquireNextImageInfoKHR pAcquireInfo, ref UInt32 pImageIndex);

        public static MgResult AcquireNextImage2KHR(VkDeviceInfo info, MgAcquireNextImageInfoKHR pAcquireInfo, ref UInt32 pImageIndex)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bSwapchain = (VkSwapchainKHR)pAcquireInfo.Swapchain;
            Debug.Assert(bSwapchain != null);

            var bSemaphore = (VkSemaphore)pAcquireInfo.Semaphore;
            var bSemaphorePtr = bSemaphore != null ? bSemaphore.Handle : 0UL;

            var bFence = (VkFence)pAcquireInfo.Fence;
            var bFencePtr = bFence != null ? bFence.Handle : 0UL;

            var bAcquireInfo = new VkAcquireNextImageInfoKHR
            {
                sType = VkStructureType.StructureTypeAcquireNextImageInfoKhr,
                // TODO: extensible
                pNext = IntPtr.Zero,
                fence = bFencePtr,
                semaphore = bSemaphorePtr,
                swapchain = bSemaphorePtr,
                timeout = pAcquireInfo.Timeout,
                deviceMask = pAcquireInfo.DeviceMask,
            };

            uint imageIndex = 0;
            var result = vkAcquireNextImage2KHR(info.Handle, bAcquireInfo, ref imageIndex);
            pImageIndex = imageIndex;
            return result;
        }
    }
}
