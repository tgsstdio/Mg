using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdResolveImageSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdResolveImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, [In, Out] VkImageResolve[] pRegions);

		public static void CmdResolveImage(VkCommandBufferInfo info, IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions)
		{
			// TODO: add implementation
		}
	}
}
