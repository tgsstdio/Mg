using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdCopyImageSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdCopyImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, [In, Out] VkImageCopy[] pRegions);

		public static void CmdCopyImage(VkCommandBufferInfo info, IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
		{
			// TODO: add implementation
		}
	}
}
