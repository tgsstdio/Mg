using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
    public partial class VkCreateSharedSwapchainsKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateSharedSwapchainsKHR(IntPtr device, UInt32 swapchainCount, [In] VkSwapchainCreateInfoKHR[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pSwapchains);

        public static MgResult CreateSharedSwapchainsKHR(VkDeviceInfo info, MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var attachedItems = new List<IntPtr>();

            try
            {

                var createInfoStructSize = Marshal.SizeOf(typeof(VkSwapchainCreateInfoKHR));
                var swapChainCount = 0U;

                if (pCreateInfos != null)
                {
                    swapChainCount = (uint)pCreateInfos.Length;
                }

                var swapChainCreateInfos = new VkSwapchainCreateInfoKHR[swapChainCount];
                for (var i = 0; i < swapChainCount; ++i)
                {
                    swapChainCreateInfos[i] = VkSwapchainCreateUtility.GenerateSwapchainCreateInfoKHR(pCreateInfos[i], attachedItems);
                }

                var sharedSwapchains = new ulong[swapChainCount];
                var result = vkCreateSharedSwapchainsKHR(info.Handle, swapChainCount, swapChainCreateInfos, allocatorPtr, sharedSwapchains);

                pSwapchains = new VkSwapchainKHR[swapChainCount];
                for (var i = 0; i < swapChainCount; ++i)
                {
                    pSwapchains[i] = new VkSwapchainKHR(sharedSwapchains[i]);
                }
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
