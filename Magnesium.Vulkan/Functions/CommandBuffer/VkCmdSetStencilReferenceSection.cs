using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetStencilReferenceSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdSetStencilReference(IntPtr commandBuffer, VkStencilFaceFlags faceMask, UInt32 reference);

        public static void CmdSetStencilReference(VkCommandBufferInfo info, MgStencilFaceFlagBits faceMask, UInt32 reference)
        {
            vkCmdSetStencilReference(info.Handle, (VkStencilFaceFlags)faceMask, reference);
        }
    }
}
