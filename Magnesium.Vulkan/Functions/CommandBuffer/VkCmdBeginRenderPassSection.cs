using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBeginRenderPassSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginRenderPass(IntPtr commandBuffer, [In, Out] VkRenderPassBeginInfo pRenderPassBegin, VkSubpassContents contents);

		public static void CmdBeginRenderPass(VkCommandBufferInfo info, MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
		{
			// TODO: add implementation
		}
	}
}
