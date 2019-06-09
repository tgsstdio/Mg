using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBlitImageSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBlitImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, [In, Out] VkImageBlit[] pRegions, VkFilter filter);

		public static void CmdBlitImage(VkCommandBufferInfo info, IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter)
		{
			// TODO: add implementation
		}
	}
}
