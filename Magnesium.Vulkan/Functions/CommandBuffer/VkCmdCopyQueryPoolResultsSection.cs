using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdCopyQueryPoolResultsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdCopyQueryPoolResults(IntPtr commandBuffer, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount, UInt64 dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags);

        public static void CmdCopyQueryPoolResults(VkCommandBufferInfo info, IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags)
        {
            var bQueryPool = (VkQueryPool)queryPool;
            Debug.Assert(bQueryPool != null);

            var bDstBuffer = (VkBuffer)dstBuffer;
            Debug.Assert(bDstBuffer != null);

            vkCmdCopyQueryPoolResults(info.Handle, bQueryPool.Handle, firstQuery, queryCount, bDstBuffer.Handle, dstOffset, stride, flags);
        }
    }
}
