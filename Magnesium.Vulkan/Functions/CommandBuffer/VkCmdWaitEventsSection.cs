using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdWaitEventsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdWaitEvents(IntPtr commandBuffer, UInt32 eventCount, UInt64* pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, UInt32 memoryBarrierCount, VkMemoryBarrier* pMemoryBarriers, UInt32 bufferMemoryBarrierCount, VkBufferMemoryBarrier* pBufferMemoryBarriers, UInt32 imageMemoryBarrierCount, VkImageMemoryBarrier* pImageMemoryBarriers);

        public static void CmdWaitEvents(VkCommandBufferInfo info, IMgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
        {
            unsafe
            {
                var eventHandles = stackalloc UInt64[pEvents.Length];
                var eventCount = (uint)pEvents.Length;
                for (var i = 0; i < eventCount; ++i)
                {
                    var bEvent = (VkEvent)pEvents[i];
                    Debug.Assert(bEvent != null);
                    eventHandles[i] = bEvent.Handle;
                }

                var memBarrierCount = 0U;
                VkMemoryBarrier* pMemBarriers = null;
                if (pMemoryBarriers != null)
                {
                    memBarrierCount = (uint)pMemoryBarriers.Length;
                    var tempMem = stackalloc VkMemoryBarrier[pMemoryBarriers.Length];
                    for (var i = 0; i < memBarrierCount; ++i)
                    {
                        tempMem[i] = new VkMemoryBarrier
                        {
                            sType = VkStructureType.StructureTypeMemoryBarrier,
                            pNext = IntPtr.Zero,
                            srcAccessMask = pMemoryBarriers[i].SrcAccessMask,
                            dstAccessMask = pMemoryBarriers[i].DstAccessMask,
                        };
                    }
                    pMemBarriers = tempMem;
                }

                uint bufBarrierCount = 0;
                VkBufferMemoryBarrier* pBufBarriers = null;
                if (pBufferMemoryBarriers != null)
                {
                    bufBarrierCount = (uint)pBufferMemoryBarriers.Length;
                    var tempBuf = stackalloc VkBufferMemoryBarrier[pBufferMemoryBarriers.Length];
                    for (var i = 0; i < bufBarrierCount; ++i)
                    {
                        var current = pBufferMemoryBarriers[i];
                        var bBuffer = (VkBuffer)current.Buffer;
                        Debug.Assert(bBuffer != null);

                        tempBuf[i] = new VkBufferMemoryBarrier
                        {
                            sType = VkStructureType.StructureTypeBufferMemoryBarrier,
                            pNext = IntPtr.Zero,
                            dstAccessMask = current.DstAccessMask,
                            srcAccessMask = current.SrcAccessMask,
                            srcQueueFamilyIndex = current.SrcQueueFamilyIndex,
                            dstQueueFamilyIndex = current.DstQueueFamilyIndex,
                            buffer = bBuffer.Handle,
                            offset = current.Offset,
                            size = current.Size,
                        };
                    }
                    pBufBarriers = tempBuf;
                }

                uint imgBarriersCount = 0;
                VkImageMemoryBarrier* pImgBarriers = null;

                if (pImageMemoryBarriers != null)
                {
                    imgBarriersCount = (uint)pImageMemoryBarriers.Length;
                    var tempImg = stackalloc VkImageMemoryBarrier[pImageMemoryBarriers.Length];
                    for (var i = 0; i < bufBarrierCount; ++i)
                    {
                        var current = pImageMemoryBarriers[i];
                        var bImage = (VkImage)current.Image;
                        Debug.Assert(bImage != null);

                        tempImg[i] = new VkImageMemoryBarrier
                        {
                            sType = VkStructureType.StructureTypeImageMemoryBarrier,
                            pNext = IntPtr.Zero,
                            dstAccessMask = current.DstAccessMask,
                            srcAccessMask = current.SrcAccessMask,
                            oldLayout = current.OldLayout,
                            newLayout = current.NewLayout,
                            srcQueueFamilyIndex = current.SrcQueueFamilyIndex,
                            dstQueueFamilyIndex = current.DstQueueFamilyIndex,
                            image = bImage.Handle,
                            subresourceRange = new VkImageSubresourceRange
                            {
                                aspectMask = current.SubresourceRange.AspectMask,
                                baseArrayLayer = current.SubresourceRange.BaseArrayLayer,
                                baseMipLevel = current.SubresourceRange.BaseMipLevel,
                                layerCount = current.SubresourceRange.LayerCount,
                                levelCount = current.SubresourceRange.LevelCount,
                            }
                        };
                    }
                    pImgBarriers = tempImg;
                }

                vkCmdWaitEvents(
                    info.Handle,
                    eventCount,
                    eventHandles,
                    srcStageMask,
                    dstStageMask,
                    memBarrierCount,
                    pMemBarriers,
                    bufBarrierCount,
                    pBufBarriers,
                    imgBarriersCount,
                    pImgBarriers);
            }

        }
    }
}
