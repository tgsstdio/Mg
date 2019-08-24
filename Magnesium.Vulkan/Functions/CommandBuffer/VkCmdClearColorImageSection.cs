using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdClearColorImageSection
	{
        [DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdClearColorImage(IntPtr commandBuffer, UInt64 image, MgImageLayout imageLayout, [In] MgClearColorValue pColor, UInt32 rangeCount, [In] Magnesium.MgImageSubresourceRange[] pRanges);

        public static void CmdClearColorImage(VkCommandBufferInfo info, IMgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges)
        {
            var bImage = (VkImage)image;
            Debug.Assert(bImage != null);

            vkCmdClearColorImage(info.Handle, bImage.Handle, imageLayout, pColor, (uint)pRanges.Length, pRanges);
        }
    }
}
