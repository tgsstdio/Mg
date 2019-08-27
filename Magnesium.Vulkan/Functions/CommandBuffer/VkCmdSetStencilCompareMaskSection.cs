using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdSetStencilCompareMaskSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdSetStencilCompareMask(IntPtr commandBuffer, VkStencilFaceFlags faceMask, UInt32 compareMask);

        public static void CmdSetStencilCompareMask(VkCommandBufferInfo info, MgStencilFaceFlagBits faceMask, UInt32 compareMask)
        {
            vkCmdSetStencilCompareMask(info.Handle, (VkStencilFaceFlags)faceMask, compareMask);
        }
    }
}
