using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBeginConditionalRenderingEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginConditionalRenderingEXT(IntPtr commandBuffer, [In, Out] VkConditionalRenderingBeginInfoEXT pConditionalRenderingBegin);

		public static void CmdBeginConditionalRenderingEXT(VkCommandBufferInfo info, MgConditionalRenderingBeginInfoEXT pConditionalRenderingBegin)
		{
			// TODO: add implementation
		}
	}
}
