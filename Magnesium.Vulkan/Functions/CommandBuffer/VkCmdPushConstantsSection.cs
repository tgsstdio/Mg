using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdPushConstantsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdPushConstants(IntPtr commandBuffer, UInt64 layout, VkShaderStageFlags stageFlags, UInt32 offset, UInt32 size, IntPtr pValues);

        public static void CmdPushConstants(VkCommandBufferInfo info, IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, UInt32 offset, UInt32 size, IntPtr pValues)
        {
            var bLayout = (VkPipelineLayout)layout;
            Debug.Assert(bLayout != null);

            vkCmdPushConstants(info.Handle, bLayout.Handle, (Magnesium.Vulkan.VkShaderStageFlags)stageFlags, offset, size, pValues);
        }
    }
}
