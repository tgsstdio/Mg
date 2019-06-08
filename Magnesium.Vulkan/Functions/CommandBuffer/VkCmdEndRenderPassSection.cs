using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdEndRenderPassSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdEndRenderPass(IntPtr commandBuffer);

		public static void CmdEndRenderPass(VkCommandBufferInfo info)
		{
			// TODO: add implementation
		}
	}
}
