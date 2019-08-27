using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdBindPipelineSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdBindPipeline(IntPtr commandBuffer, MgPipelineBindPoint pipelineBindPoint, UInt64 pipeline);

        public static void CmdBindPipeline(VkCommandBufferInfo info, MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
        {
            var bPipeline = (VkPipeline)pipeline;
            Debug.Assert(bPipeline != null);

            vkCmdBindPipeline(info.Handle, pipelineBindPoint, bPipeline.Handle);
        }
    }
}
