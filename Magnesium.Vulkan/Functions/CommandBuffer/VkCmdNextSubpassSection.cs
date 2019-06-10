using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdNextSubpassSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdNextSubpass(IntPtr commandBuffer, VkSubpassContents contents);

        public static void CmdNextSubpass(VkCommandBufferInfo info, MgSubpassContents contents)
        {
            vkCmdNextSubpass(info.Handle, (VkSubpassContents)contents);
        }
    }
}
