using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdBindDescriptorSetsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdBindDescriptorSets(IntPtr commandBuffer, MgPipelineBindPoint pipelineBindPoint, UInt64 layout, UInt32 firstSet, UInt32 descriptorSetCount, [In] UInt64[] pDescriptorSets, UInt32 dynamicOffsetCount, [In] UInt32[] pDynamicOffsets);

        public static void CmdBindDescriptorSets(VkCommandBufferInfo info, MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, UInt32 firstSet, IMgDescriptorSet[] pDescriptorSets, UInt32[] pDynamicOffsets)
        {
            var bLayout = (VkPipelineLayout)layout;
            Debug.Assert(bLayout != null);

            var stride = Marshal.SizeOf(typeof(IntPtr));
            var descriptorSetCount = pDescriptorSets != null ? (uint)pDescriptorSets.Length : 0U;
            IntPtr sets = Marshal.AllocHGlobal((int)(stride * descriptorSetCount));

            var src = new ulong[descriptorSetCount];
            for (uint i = 0; i < descriptorSetCount; ++i)
            {
                var bDescSet = (VkDescriptorSet)pDescriptorSets[i];
                Debug.Assert(bDescSet != null);
                src[i] = bDescSet.Handle;
            }

            // var dynamic (uint)pDynamicOffsets.Length
            uint dynamicOffsetCount = pDynamicOffsets != null ? (uint)pDynamicOffsets.Length : 0U;
            vkCmdBindDescriptorSets(info.Handle, pipelineBindPoint, bLayout.Handle, firstSet, descriptorSetCount, src, dynamicOffsetCount, pDynamicOffsets);
        }
    }
}
