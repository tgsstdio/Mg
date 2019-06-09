using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdClearColorImageSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static void vkCmdClearColorImage(IntPtr commandBuffer, UInt64 image, MgImageLayout imageLayout, [In, Out] VkClearColorValue pColor, UInt32 rangeCount, [In, Out] VkImageSubresourceRange[] pRanges);

		public static void CmdClearColorImage(VkCommandBufferInfo info, IMgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges)
		{
			// TODO: add implementation
		}
	}
}
