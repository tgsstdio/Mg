using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdEndRenderPassSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdEndRenderPass(IntPtr commandBuffer);

        public static void CmdEndRenderPass(VkCommandBufferInfo info)
        {
            vkCmdEndRenderPass(info.Handle);
        }
    }
}
