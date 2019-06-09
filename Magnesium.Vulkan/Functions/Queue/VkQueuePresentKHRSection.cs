using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueuePresentKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkQueuePresentKHR(IntPtr queue, ref VkPresentInfoKHR pPresentInfo);

        public static MgResult QueuePresentKHR(VkQueueInfo info, MgPresentInfoKHR pPresentInfo)
        {
            var attachedItems = new List<IntPtr>();
            try
            {
                uint waitSemaphoreCount;
                var pWaitSemaphores = VkQueueUtility.ExtractSemaphores(attachedItems, pPresentInfo.WaitSemaphores, out waitSemaphoreCount);

                IntPtr pSwapchains;
                IntPtr pImageIndices;
                uint swapchainCount = ExtractSwapchains(attachedItems, pPresentInfo.Images, out pSwapchains, out pImageIndices);

                var pResults = ExtractResults(attachedItems, pPresentInfo.Results);

                var presentInfo = new VkPresentInfoKHR
                {
                    sType = VkStructureType.StructureTypePresentInfoKhr,
                    pNext = IntPtr.Zero,
                    waitSemaphoreCount = waitSemaphoreCount,
                    pWaitSemaphores = pWaitSemaphores,
                    swapchainCount = swapchainCount,
                    pSwapchains = pSwapchains,
                    pImageIndices = pImageIndices,
                    pResults = pResults,
                };

                var result = vkQueuePresentKHR(info.Handle, ref presentInfo);

                // MUST ABLE TO RETURN 
                if (pResults != IntPtr.Zero)
                {
                    var stride = Marshal.SizeOf(typeof(MgResult));
                    var swapChains = new MgResult[swapchainCount];
                    var offset = 0;
                    for (var i = 0; i < swapchainCount; ++i)
                    {
                        var src = IntPtr.Add(pResults, offset);
                        swapChains[i] = (Magnesium.MgResult)Marshal.PtrToStructure(src, typeof(Magnesium.MgResult));
                        offset += stride;
                    }

                    pPresentInfo.Results = swapChains;
                }

                return result;
            }
            finally
            {
                foreach (var item in attachedItems)
                {
                    Marshal.FreeHGlobal(item);
                }
            }
        }

        static IntPtr ExtractResults(List<IntPtr> attachedItems, MgResult[] results)
        {
            if (results == null)
                return IntPtr.Zero;

            var stride = Marshal.SizeOf(typeof(MgResult));
            var dest = Marshal.AllocHGlobal(stride * results.Length);
            attachedItems.Add(dest);
            return dest;
        }

        static uint ExtractSwapchains(List<IntPtr> attachedItems, MgPresentInfoKHRImage[] images, out IntPtr swapchains, out IntPtr imageIndices)
        {
            var pSwapchains = IntPtr.Zero;
            var pImageIndices = IntPtr.Zero;
            uint count = 0U;

            if (images != null)
            {
                count = (uint)images.Length;
                if (count > 0)
                {
                    pSwapchains = VkInteropsUtility.ExtractUInt64HandleArray(
                        images,
                        (sc) =>
                        {
                            var bSwapchain = (VkSwapchainKHR)sc.Swapchain;
                            Debug.Assert(bSwapchain != null);
                            return bSwapchain.Handle;
                        });
                    attachedItems.Add(pSwapchains);

                    pImageIndices = VkInteropsUtility.AllocateHGlobalArray(
                        images,
                        (sc) => { return sc.ImageIndex; });
                    attachedItems.Add(pImageIndices);
                }
            }
            swapchains = pSwapchains;
            imageIndices = pImageIndices;
            return count;
        }
    }
}
