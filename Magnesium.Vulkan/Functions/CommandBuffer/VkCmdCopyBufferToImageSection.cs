using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdCopyBufferToImageSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdCopyBufferToImage(IntPtr commandBuffer, UInt64 srcBuffer, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, [In, Out] VkBufferImageCopy[] pRegions);

		public static void CmdCopyBufferToImage(VkCommandBufferInfo info, IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
		{
			// TODO: add implementation
		}
	}
}
