using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateSwapchainKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateSwapchainKHR(IntPtr device, ref VkSwapchainCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSwapchain);

        public static MgResult CreateSwapchainKHR(VkDeviceInfo info, MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            var attachedItems = new List<IntPtr>();

            try
            {
                var createInfo = VkSwapchainCreateUtility.GenerateSwapchainCreateInfoKHR(pCreateInfo, attachedItems);

                ulong internalHandle = 0;
                var result = vkCreateSwapchainKHR(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pSwapchain = new VkSwapchainKHR(internalHandle);
                return result;
            }
            finally
            {
                foreach (var handle in attachedItems)
                {
                    Marshal.FreeHGlobal(handle);
                }
            }
        }
    }
}
