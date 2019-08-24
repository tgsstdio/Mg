using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdPipelineBarrierSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdPipelineBarrier(IntPtr commandBuffer, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, UInt32 memoryBarrierCount, VkMemoryBarrier* pMemoryBarriers, UInt32 bufferMemoryBarrierCount, VkBufferMemoryBarrier* pBufferMemoryBarriers, UInt32 imageMemoryBarrierCount, VkImageMemoryBarrier* pImageMemoryBarriers);

        public static void CmdPipelineBarrier(VkCommandBufferInfo info, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
        {
            unsafe
            {
                var memBarrierCount = pMemoryBarriers != null ? (uint)pMemoryBarriers.Length : 0U;
                var pMemBarriers = stackalloc VkMemoryBarrier[(int)memBarrierCount];

                for (var i = 0; i < memBarrierCount; ++i)
                {
                    pMemBarriers[i] = new VkMemoryBarrier
                    {
                        sType = VkStructureType.StructureTypeMemoryBarrier,
                        pNext = IntPtr.Zero,
                        srcAccessMask = pMemoryBarriers[i].SrcAccessMask,
                        dstAccessMask = pMemoryBarriers[i].DstAccessMask,
                    };
                }


                uint bufBarrierCount = pBufferMemoryBarriers != null ? (uint)pBufferMemoryBarriers.Length : 0U;
                var pBufBarriers = stackalloc VkBufferMemoryBarrier[(int)bufBarrierCount];
                for (var j = 0; j < bufBarrierCount; ++j)
                {
                    var current = pBufferMemoryBarriers[j];
                    var bBuffer = (VkBuffer)current.Buffer;
                    Debug.Assert(bBuffer != null);

                    pBufBarriers[j] = new VkBufferMemoryBarrier
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


                uint imgBarriersCount = pImageMemoryBarriers != null ? (uint)pImageMemoryBarriers.Length : 0U;
                var pImgBarriers = stackalloc VkImageMemoryBarrier[(int)imgBarriersCount];

                for (var k = 0; k < imgBarriersCount; ++k)
                {
                    var current = pImageMemoryBarriers[k];
                    var bImage = (VkImage)current.Image;
                    Debug.Assert(bImage != null);

                    pImgBarriers[k] = new VkImageMemoryBarrier
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

                VkMemoryBarrier* mems = memBarrierCount > 0 ? pMemBarriers : null;
                VkBufferMemoryBarrier* bufs = bufBarrierCount > 0 ? pBufBarriers : null;
                VkImageMemoryBarrier* images = imgBarriersCount > 0 ? pImgBarriers : null;


                vkCmdPipelineBarrier(info.Handle,
                      srcStageMask,
                      dstStageMask,
                      dependencyFlags,
                      memBarrierCount,
                      mems,
                      bufBarrierCount,
                      bufs,
                      imgBarriersCount,
                      images);
            }
        }
    }
}
