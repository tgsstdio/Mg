using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdResetEventSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdResetEvent(IntPtr commandBuffer, UInt64 @event, VkPipelineStageFlags stageMask);

		public static void CmdResetEvent(VkCommandBufferInfo info, IMgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			// TODO: add implementation
		}
	}
}
