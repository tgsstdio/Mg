using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetDepthBoundsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdSetDepthBounds(IntPtr commandBuffer, float minDepthBounds, float maxDepthBounds);

        public static void CmdSetDepthBounds(VkCommandBufferInfo info, float minDepthBounds, float maxDepthBounds)
        {
            vkCmdSetDepthBounds(info.Handle, minDepthBounds, maxDepthBounds);
        }
    }
}
