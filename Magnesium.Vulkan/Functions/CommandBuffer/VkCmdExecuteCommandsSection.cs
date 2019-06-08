using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdExecuteCommandsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdExecuteCommands(IntPtr commandBuffer, UInt32 commandBufferCount, IntPtr[] pCommandBuffers);

		public static void CmdExecuteCommands(VkCommandBufferInfo info, IMgCommandBuffer[] pCommandBuffers)
		{
			// TODO: add implementation
		}
	}
}
