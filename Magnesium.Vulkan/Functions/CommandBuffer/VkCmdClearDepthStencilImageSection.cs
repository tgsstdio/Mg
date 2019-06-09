using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdClearDepthStencilImageSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdClearDepthStencilImage(IntPtr commandBuffer, UInt64 image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, UInt32 rangeCount, [In, Out] VkImageSubresourceRange[] pRanges);

		public static void CmdClearDepthStencilImage(VkCommandBufferInfo info, IMgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges)
		{
			// TODO: add implementation
		}
	}
}
