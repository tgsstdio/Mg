using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdCopyQueryPoolResultsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdCopyQueryPoolResults(IntPtr commandBuffer, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount, UInt64 dstBuffer, VkDeviceSize dstOffset, VkDeviceSize stride, VkQueryResultFlags flags);

		public static void CmdCopyQueryPoolResults(VkCommandBufferInfo info, IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags)
		{
			// TODO: add implementation
		}
	}
}
