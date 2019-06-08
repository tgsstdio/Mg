using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdWriteTimestampSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdWriteTimestamp(IntPtr commandBuffer, VkPipelineStageFlagBits pipelineStage, UInt64 queryPool, UInt32 query);

		public static void CmdWriteTimestamp(VkCommandBufferInfo info, MgPipelineStageFlagBits pipelineStage, IMgQueryPool queryPool, UInt32 query)
		{
			// TODO: add implementation
		}
	}
}
