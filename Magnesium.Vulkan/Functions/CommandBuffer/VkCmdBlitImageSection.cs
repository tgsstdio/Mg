using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBlitImageSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdBlitImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, [In, Out] MgImageBlit[] pRegions, MgFilter filter);

        public static void CmdBlitImage(VkCommandBufferInfo info, IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter)
        {
            var bSrcImage = (VkImage)srcImage;
            Debug.Assert(bSrcImage != null);
            var bDstImage = (VkImage)dstImage;
            Debug.Assert(bDstImage != null);

            vkCmdBlitImage(info.Handle, bSrcImage.Handle, srcImageLayout, bDstImage.Handle, dstImageLayout, (uint)pRegions.Length, pRegions, filter);
        }
    }
}
