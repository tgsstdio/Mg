using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBindPipelineSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindPipeline(IntPtr commandBuffer, MgPipelineBindPoint pipelineBindPoint, UInt64 pipeline);

		public static void CmdBindPipeline(VkCommandBufferInfo info, MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
		{
			// TODO: add implementation
		}
	}
}
