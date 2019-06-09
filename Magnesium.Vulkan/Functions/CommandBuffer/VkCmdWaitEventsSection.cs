using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdWaitEventsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdWaitEvents(IntPtr commandBuffer, UInt32 eventCount, UInt64[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, UInt32 memoryBarrierCount, [In, Out] VkMemoryBarrier[] pMemoryBarriers, UInt32 bufferMemoryBarrierCount, [In, Out] VkBufferMemoryBarrier[] pBufferMemoryBarriers, UInt32 imageMemoryBarrierCount, [In, Out] VkImageMemoryBarrier[] pImageMemoryBarriers);

		public static void CmdWaitEvents(VkCommandBufferInfo info, IMgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			// TODO: add implementation
		}
	}
}
