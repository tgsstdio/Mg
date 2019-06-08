using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdProcessCommandsNVXSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdProcessCommandsNVX(IntPtr commandBuffer, VkCmdProcessCommandsInfoNVX pProcessCommandsInfo);

		public static void CmdProcessCommandsNVX(VkCommandBufferInfo info, MgCmdProcessCommandsInfoNVX pProcessCommandsInfo)
		{
			// TODO: add implementation
		}
	}
}
