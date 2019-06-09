using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetEventSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetEvent(IntPtr commandBuffer, UInt64 @event, MgPipelineStageFlagBits stageMask);

		public static void CmdSetEvent(VkCommandBufferInfo info, IMgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			// TODO: add implementation
		}
	}
}
