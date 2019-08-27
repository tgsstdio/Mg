using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkCreatePipelineLayoutSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreatePipelineLayout(IntPtr device, ref VkPipelineLayoutCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pPipelineLayout);

        public static MgResult CreatePipelineLayout(VkDeviceInfo info, MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            var pSetLayouts = IntPtr.Zero;

            var pPushConstantRanges = IntPtr.Zero;

            try
            {

                var setLayoutCount = 0U;
                if (pCreateInfo.SetLayouts != null)
                {
                    setLayoutCount = (UInt32)pCreateInfo.SetLayouts.Length;
                    if (setLayoutCount > 0)
                    {
                        pSetLayouts = VkInteropsUtility.ExtractUInt64HandleArray(pCreateInfo.SetLayouts,
                            (dsl) =>
                            {
                                var bDescriptorSetLayout = (VkDescriptorSetLayout)dsl;
                                Debug.Assert(bDescriptorSetLayout != null);
                                return bDescriptorSetLayout.Handle;
                            });
                    }
                }

                var pushConstantRangeCount = 0U;
                if (pCreateInfo.PushConstantRanges != null)
                {
                    pushConstantRangeCount = (UInt32)pCreateInfo.PushConstantRanges.Length;

                    if (pushConstantRangeCount > 0)
                    {
                        pPushConstantRanges = VkInteropsUtility.AllocateHGlobalArray
                            (
                                pCreateInfo.PushConstantRanges,
                                (pcr) =>
                                {
                                    return new VkPushConstantRange
                                    {
                                        stageFlags = pcr.StageFlags,
                                        offset = pcr.Offset,
                                        size = pcr.Size,
                                    };
                                }
                            );
                    }
                }

                ulong internalHandle = 0;
                var createInfo = new VkPipelineLayoutCreateInfo
                {
                    sType = VkStructureType.StructureTypePipelineLayoutCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    setLayoutCount = setLayoutCount,
                    pSetLayouts = pSetLayouts,
                    pushConstantRangeCount = pushConstantRangeCount,
                    pPushConstantRanges = pPushConstantRanges,

                };
                var result = vkCreatePipelineLayout(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pPipelineLayout = new VkPipelineLayout(internalHandle);
                return result;
            }
            finally
            {
                if (pSetLayouts != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pSetLayouts);
                }

                if (pPushConstantRanges != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pPushConstantRanges);
                }
            }
        }
    }
}
