using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdSetStencilWriteMaskSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdSetStencilWriteMask(IntPtr commandBuffer, VkStencilFaceFlags faceMask, UInt32 writeMask);

        public static void CmdSetStencilWriteMask(VkCommandBufferInfo info, MgStencilFaceFlagBits faceMask, UInt32 writeMask)
        {
            vkCmdSetStencilWriteMask(info.Handle, (VkStencilFaceFlags)faceMask, writeMask);
        }
    }
}
