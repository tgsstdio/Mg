using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateDescriptorUpdateTemplateSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateDescriptorUpdateTemplate(IntPtr device, [In, Out] VkDescriptorUpdateTemplateCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pDescriptorUpdateTemplate);

        public static MgResult CreateDescriptorUpdateTemplate(VkDeviceInfo info, MgDescriptorUpdateTemplateCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgDescriptorUpdateTemplate pDescriptorUpdateTemplate)
        {
            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(pAllocator);

            var descriptorUpdateEntryCount = (UInt32)pCreateInfo.DescriptorUpdateEntries.Length;

            var pDescriptorUpdateEntries = IntPtr.Zero;

            try
            {
                pDescriptorUpdateEntries = VkInteropsUtility.AllocateHGlobalArray(
                    pCreateInfo.DescriptorUpdateEntries,
                    (src) =>
                    {
                        return new VkDescriptorUpdateTemplateEntry
                        {
                            dstBinding = src.DstBinding,
                            dstArrayElement = src.DstArrayElement,
                            descriptorCount = src.DescriptorCount,
                            descriptorType = src.DescriptorType,
                            offset = src.Offset,
                            stride = src.Stride,
                        };
                    }
                );

                var bSetLayout = (VkDescriptorSetLayout)pCreateInfo.DescriptorSetLayout;
                var bSetLayoutPtr = bSetLayout != null ? bSetLayout.Handle : 0UL;

                var bPipelineLayout = (VkPipelineLayout)pCreateInfo.PipelineLayout;
                var bPipelineLayoutPtr = bPipelineLayout != null ? bPipelineLayout.Handle : 0UL;

                var bCreateInfo = new VkDescriptorUpdateTemplateCreateInfo
                {
                    sType = VkStructureType.StructureTypeDescriptorUpdateTemplateCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    descriptorUpdateEntryCount = descriptorUpdateEntryCount,
                    pDescriptorUpdateEntries = pDescriptorUpdateEntries,
                    templateType = pCreateInfo.TemplateType,
                    descriptorSetLayout = bSetLayoutPtr,
                    pipelineBindPoint = pCreateInfo.PipelineBindPoint,
                    pipelineLayout = bPipelineLayoutPtr,
                    set = pCreateInfo.Set,
                };

                var pHandle = 0UL;
                var result = vkCreateDescriptorUpdateTemplate(info.Handle, bCreateInfo, allocatorPtr, ref pHandle);

                pDescriptorUpdateTemplate = new VkDescriptorUpdateTemplate(pHandle);
                return result;
            }
            finally
            {
                if (pDescriptorUpdateEntries != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pDescriptorUpdateEntries);
                }
            }
        }
    }
}
