using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdWriteTimestampSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdWriteTimestamp(IntPtr commandBuffer, MgPipelineStageFlagBits pipelineStage, UInt64 queryPool, UInt32 query);

        public static void CmdWriteTimestamp(VkCommandBufferInfo info, MgPipelineStageFlagBits pipelineStage, IMgQueryPool queryPool, UInt32 query)
        {
            var bQueryPool = (VkQueryPool)queryPool;
            Debug.Assert(bQueryPool != null);

            vkCmdWriteTimestamp(info.Handle, pipelineStage, bQueryPool.Handle, query);
        }
    }
}
