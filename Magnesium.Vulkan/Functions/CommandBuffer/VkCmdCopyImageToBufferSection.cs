using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdCopyImageToBufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdCopyImageToBuffer(IntPtr commandBuffer, UInt64 srcImage, VkImageLayout srcImageLayout, UInt64 dstBuffer, UInt32 regionCount, [In, Out] VkBufferImageCopy[] pRegions);

		public static void CmdCopyImageToBuffer(VkCommandBufferInfo info, IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
		{
			// TODO: add implementation
		}
	}
}
