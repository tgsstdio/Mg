using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdPushConstantsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdPushConstants(IntPtr commandBuffer, UInt64 layout, VkShaderStageFlags stageFlags, UInt32 offset, UInt32 size, IntPtr[] pValues);

		public static void CmdPushConstants(VkCommandBufferInfo info, IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, UInt32 offset, UInt32 size, IntPtr pValues)
		{
			// TODO: add implementation
		}
	}
}
