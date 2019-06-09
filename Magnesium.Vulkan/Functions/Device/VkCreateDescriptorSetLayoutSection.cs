using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateDescriptorSetLayoutSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateDescriptorSetLayout(IntPtr device, ref VkDescriptorSetLayoutCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSetLayout);

        public static MgResult CreateDescriptorSetLayout(VkDeviceInfo info, MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var attachedItems = new List<IntPtr>();

            try
            {
                var bindingCount = 0U;
                var pBindings = IntPtr.Zero;

                if (pCreateInfo.Bindings != null)
                {
                    bindingCount = (uint)pCreateInfo.Bindings.Length;
                    if (bindingCount > 0)
                    {
                        var stride = Marshal.SizeOf(typeof(VkDescriptorSetLayoutBinding));
                        pBindings = Marshal.AllocHGlobal((int)(bindingCount * stride));
                        attachedItems.Add(pBindings);

                        var offset = 0;
                        foreach (var currentBinding in pCreateInfo.Bindings)
                        {
                            /**
							 * TODO:
							 * If descriptorType is VK_DESCRIPTOR_TYPE_SAMPLER or VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
							 * and descriptorCount is not 0 and pImmutableSamplers is not NULL, pImmutableSamplers must be a 
							 * pointer to an array of descriptorCount valid VkSampler handles
							**/

                            var pImmutableSamplers = IntPtr.Zero;
                            if (currentBinding.ImmutableSamplers != null)
                            {
                                if (currentBinding.DescriptorCount > 0)
                                {
                                    var arraySize = (int)(currentBinding.DescriptorCount * sizeof(UInt64));
                                    pImmutableSamplers = VkInteropsUtility.ExtractUInt64HandleArray(currentBinding.ImmutableSamplers,
                                            (sampler) =>
                                            {
                                                var bSampler = (VkSampler)sampler;
                                                Debug.Assert(bSampler != null);
                                                return bSampler.Handle;
                                            }
                                        );

                                    attachedItems.Add(pImmutableSamplers);
                                }
                            }

                            var binding = new VkDescriptorSetLayoutBinding
                            {
                                binding = currentBinding.Binding,
                                descriptorType = currentBinding.DescriptorType,
                                descriptorCount = currentBinding.DescriptorCount,
                                stageFlags = (VkShaderStageFlags)currentBinding.StageFlags,
                                pImmutableSamplers = pImmutableSamplers,
                            };

                            var dest = IntPtr.Add(pBindings, offset);
                            Marshal.StructureToPtr(binding, dest, false);
                            offset += stride;
                        }
                    }
                }


                var internalHandle = 0UL;
                var createInfo = new VkDescriptorSetLayoutCreateInfo
                {
                    sType = VkStructureType.StructureTypeDescriptorSetLayoutCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    bindingCount = bindingCount,
                    pBindings = pBindings,
                };
                var result = vkCreateDescriptorSetLayout(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pSetLayout = new VkDescriptorSetLayout(internalHandle);
                return result;
            }
            finally
            {
                foreach (var handle in attachedItems)
                {
                    Marshal.FreeHGlobal(handle);
                }
            }
        }
    }
}
