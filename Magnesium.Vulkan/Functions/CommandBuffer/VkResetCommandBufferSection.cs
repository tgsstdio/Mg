using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkResetCommandBufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkResetCommandBuffer(IntPtr commandBuffer, VkCommandBufferResetFlags flags);

        public static MgResult ResetCommandBuffer(VkCommandBufferInfo info, MgCommandBufferResetFlagBits flags)
        {
            return vkResetCommandBuffer(info.Handle, (VkCommandBufferResetFlags)flags);
        }
    }
}
