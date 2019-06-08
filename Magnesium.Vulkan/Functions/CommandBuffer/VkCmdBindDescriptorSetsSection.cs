using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdBindDescriptorSetsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindDescriptorSets(IntPtr commandBuffer, VkPipelineBindPoint pipelineBindPoint, UInt64 layout, UInt32 firstSet, UInt32 descriptorSetCount, UInt64[] pDescriptorSets, UInt32 dynamicOffsetCount, UInt32[] pDynamicOffsets);

		public static void CmdBindDescriptorSets(VkCommandBufferInfo info, MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, UInt32 firstSet, UInt32 descriptorSetCount, IMgDescriptorSet[] pDescriptorSets, UInt32[] pDynamicOffsets)
		{
			// TODO: add implementation
		}
	}
}
