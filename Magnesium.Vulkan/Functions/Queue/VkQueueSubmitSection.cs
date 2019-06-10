using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueueSubmitSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe MgResult vkQueueSubmit(IntPtr queue, UInt32 submitCount, VkSubmitInfo* pSubmits, UInt64 fence);

        public static MgResult QueueSubmit(VkQueueInfo info, MgSubmitInfo[] pSubmits, IMgFence fence)
        {
            var bFence = (VkFence)fence;
            var bFencePtr = bFence != null ? bFence.Handle : 0UL;

            var attachedItems = new List<IntPtr>();
            try
            {
                unsafe
                {
                    uint submitCount = 0;

                    if (pSubmits != null)
                    {
                        submitCount = (uint)pSubmits.Length;
                    }

                    var submissions = stackalloc VkSubmitInfo[(int)submitCount];

                    for (var i = 0; i < submitCount; ++i)
                    {
                        var currentInfo = pSubmits[i];

                        var waitSemaphoreCount = 0U;
                        var pWaitSemaphores = IntPtr.Zero;
                        var pWaitDstStageMask = IntPtr.Zero;

                        if (currentInfo.WaitSemaphores != null)
                        {
                            waitSemaphoreCount = (uint)currentInfo.WaitSemaphores.Length;
                            if (waitSemaphoreCount > 0)
                            {
                                pWaitSemaphores = VkInteropsUtility.ExtractUInt64HandleArray(
                                    currentInfo.WaitSemaphores,
                                    (arg) =>
                                    {
                                        var bSemaphore = (VkSemaphore)arg.WaitSemaphore;
                                        Debug.Assert(bSemaphore != null);
                                        return bSemaphore.Handle;
                                    });
                                attachedItems.Add(pWaitSemaphores);

                                pWaitDstStageMask = VkInteropsUtility.AllocateHGlobalArray<MgSubmitInfoWaitSemaphoreInfo, uint>(
                                    currentInfo.WaitSemaphores,
                                    (arg) => { return (uint)arg.WaitDstStageMask; });
                                attachedItems.Add(pWaitDstStageMask);
                            }
                        }

                        var commandBufferCount = 0U;
                        var pCommandBuffers = IntPtr.Zero;

                        if (currentInfo.CommandBuffers != null)
                        {
                            commandBufferCount = (uint)currentInfo.CommandBuffers.Length;
                            if (commandBufferCount > 0)
                            {
                                pCommandBuffers = VkInteropsUtility.ExtractIntPtrHandleArray
                                (
                                    currentInfo.CommandBuffers,
                                    (arg) =>
                                    {
                                        var bCommandBuffer = (VkCommandBuffer)arg;
                                        Debug.Assert(bCommandBuffer != null);
                                        return bCommandBuffer.Info.Handle;
                                    }
                                  );
                                attachedItems.Add(pCommandBuffers);
                            }
                        }

                        var signalSemaphoreCount = 0U;
                        var pSignalSemaphores = IntPtr.Zero;

                        if (currentInfo.SignalSemaphores != null)
                        {
                            signalSemaphoreCount = (uint)currentInfo.SignalSemaphores.Length;

                            if (signalSemaphoreCount > 0)
                            {
                                pSignalSemaphores = VkInteropsUtility.ExtractUInt64HandleArray(
                                    currentInfo.SignalSemaphores,
                                    (arg) =>
                                    {
                                        var bSemaphore = (VkSemaphore)arg;
                                        Debug.Assert(bSemaphore != null);
                                        return bSemaphore.Handle;
                                    });
                                attachedItems.Add(pSignalSemaphores);
                            }
                        }

                        submissions[i] = new VkSubmitInfo
                        {
                            sType = VkStructureType.StructureTypeSubmitInfo,
                            pNext = IntPtr.Zero,
                            waitSemaphoreCount = waitSemaphoreCount,
                            pWaitSemaphores = pWaitSemaphores,
                            pWaitDstStageMask = pWaitDstStageMask,
                            commandBufferCount = commandBufferCount,
                            pCommandBuffers = pCommandBuffers,
                            signalSemaphoreCount = signalSemaphoreCount,
                            pSignalSemaphores = pSignalSemaphores,
                        };
                    }

                    return vkQueueSubmit(info.Handle, submitCount, submitCount > 0 ? submissions : null, bFencePtr);
                }
            }
            finally
            {
                foreach (var item in attachedItems)
                {
                    Marshal.FreeHGlobal(item);
                }
            }
        }
    }
}
