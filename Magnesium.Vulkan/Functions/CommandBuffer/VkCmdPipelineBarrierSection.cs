using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdPipelineBarrierSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdPipelineBarrier(IntPtr commandBuffer, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, UInt32 memoryBarrierCount, [In, Out] VkMemoryBarrier[] pMemoryBarriers, UInt32 bufferMemoryBarrierCount, [In, Out] VkBufferMemoryBarrier[] pBufferMemoryBarriers, UInt32 imageMemoryBarrierCount, [In, Out] VkImageMemoryBarrier[] pImageMemoryBarriers);

		public static void CmdPipelineBarrier(VkCommandBufferInfo info, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			// TODO: add implementation
		}
	}
}
