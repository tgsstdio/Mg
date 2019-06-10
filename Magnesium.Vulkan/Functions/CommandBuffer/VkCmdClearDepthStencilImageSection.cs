using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdClearDepthStencilImageSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdClearDepthStencilImage(IntPtr commandBuffer, UInt64 image, MgImageLayout imageLayout, [In] MgClearDepthStencilValue pDepthStencil, UInt32 rangeCount, [In] Magnesium.MgImageSubresourceRange[] pRanges);

        public static void CmdClearDepthStencilImage(VkCommandBufferInfo info, IMgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges)
        {
            var bImage = (VkImage)image;
            Debug.Assert(bImage != null);

            vkCmdClearDepthStencilImage(info.Handle, bImage.Handle, imageLayout, pDepthStencil, (uint)pRanges.Length, pRanges);
        }
    }
}
