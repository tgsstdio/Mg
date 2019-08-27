using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdSetLineWidthSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdSetLineWidth(IntPtr commandBuffer, float lineWidth);

        public static void CmdSetLineWidth(VkCommandBufferInfo info, float lineWidth)
        {
            vkCmdSetLineWidth(info.Handle, lineWidth);
        }
    }
}
