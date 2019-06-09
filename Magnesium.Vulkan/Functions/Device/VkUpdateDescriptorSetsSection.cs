using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkUpdateDescriptorSetsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkUpdateDescriptorSets(IntPtr device, UInt32 descriptorWriteCount, VkWriteDescriptorSet* pDescriptorWrites, UInt32 descriptorCopyCount, VkCopyDescriptorSet* pDescriptorCopies);

        public static void UpdateDescriptorSets(VkDeviceInfo info, MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var writeCount = 0U;
            if (pDescriptorWrites != null)
            {
                writeCount = (uint)pDescriptorWrites.Length;
            }

            var copyCount = 0U;
            if (pDescriptorCopies != null)
            {
                copyCount = (uint)pDescriptorCopies.Length;
            }

            var attachedItems = new List<IntPtr>();

            try
            {
                unsafe
                {
                    VkWriteDescriptorSet* writes = null;
                    VkCopyDescriptorSet* copies = null;

                    if (writeCount > 0)
                    {
                        var bWriteSets = stackalloc VkWriteDescriptorSet[(int)writeCount];

                        for (var i = 0; i < writeCount; ++i)
                        {
                            var currentWrite = pDescriptorWrites[i];
                            var bDstSet = (VkDescriptorSet)currentWrite.DstSet;
                            Debug.Assert(bDstSet != null);

                            var descriptorCount = (int)currentWrite.DescriptorCount;

                            var pImageInfo = IntPtr.Zero;
                            if (currentWrite.ImageInfo != null)
                            {
                                if (descriptorCount > 0)
                                {
                                    pImageInfo = VkInteropsUtility.AllocateHGlobalArray(
                                        currentWrite.ImageInfo,
                                        (srcInfo) =>
                                        {
                                            var bSampler = (VkSampler)srcInfo.Sampler;
                                            Debug.Assert(bSampler != null);

                                            var bImageView = (VkImageView)srcInfo.ImageView;
                                            Debug.Assert(bImageView != null);

                                            return new VkDescriptorImageInfo
                                            {
                                                sampler = bSampler.Handle,
                                                imageView = bImageView.Handle,
                                                imageLayout = srcInfo.ImageLayout,
                                            };
                                        });
                                    attachedItems.Add(pImageInfo);
                                }
                            }

                            var pBufferInfo = IntPtr.Zero;
                            if (currentWrite.BufferInfo != null)
                            {
                                if (descriptorCount > 0)
                                {
                                    pBufferInfo = VkInteropsUtility.AllocateHGlobalArray(
                                        currentWrite.BufferInfo,
                                        (src) =>
                                        {
                                            var bBuffer = (VkBuffer)src.Buffer;
                                            Debug.Assert(bBuffer != null);

                                            return new VkDescriptorBufferInfo
                                            {
                                                buffer = bBuffer.Handle,
                                                offset = src.Offset,
                                                range = src.Range,
                                            };
                                        }
                                    );
                                    attachedItems.Add(pBufferInfo);
                                }
                            }

                            var pTexelBufferView = IntPtr.Zero;
                            if (currentWrite.TexelBufferView != null)
                            {
                                if (descriptorCount > 0)
                                {
                                    pTexelBufferView = VkInteropsUtility.ExtractUInt64HandleArray(currentWrite.TexelBufferView,
                                        (tbv) =>
                                        {
                                            var bBufferView = (VkBufferView)tbv;
                                            Debug.Assert(bBufferView != null);
                                            return bBufferView.Handle;
                                        }
                                        );
                                    attachedItems.Add(pTexelBufferView);
                                }
                            }

                            bWriteSets[i] = new VkWriteDescriptorSet
                            {
                                sType = VkStructureType.StructureTypeWriteDescriptorSet,
                                pNext = IntPtr.Zero,
                                dstSet = bDstSet.Handle,
                                dstBinding = currentWrite.DstBinding,
                                dstArrayElement = currentWrite.DstArrayElement,
                                descriptorCount = currentWrite.DescriptorCount,
                                descriptorType = currentWrite.DescriptorType,
                                pImageInfo = pImageInfo,
                                pBufferInfo = pBufferInfo,
                                pTexelBufferView = pTexelBufferView,
                            };
                        }

                        writes = bWriteSets;
                    }

                    if (copyCount > 0)
                    {
                        var bCopySets = stackalloc VkCopyDescriptorSet[(int)copyCount];

                        for (var j = 0; j < copyCount; ++j)
                        {
                            var currentCopy = pDescriptorCopies[j];

                            var bSrcSet = (VkDescriptorSet)currentCopy.SrcSet;
                            Debug.Assert(bSrcSet != null);

                            var bDstSet = (VkDescriptorSet)currentCopy.DstSet;
                            Debug.Assert(bDstSet != null);

                            bCopySets[j] = new VkCopyDescriptorSet
                            {
                                sType = VkStructureType.StructureTypeCopyDescriptorSet,
                                pNext = IntPtr.Zero,
                                srcSet = bSrcSet.Handle,
                                srcBinding = currentCopy.SrcBinding,
                                srcArrayElement = currentCopy.SrcArrayElement,
                                dstSet = bDstSet.Handle,
                                dstBinding = currentCopy.DstBinding,
                                dstArrayElement = currentCopy.DstArrayElement,
                                descriptorCount = currentCopy.DescriptorCount,
                            };
                        }

                        copies = bCopySets;
                    }

                    vkUpdateDescriptorSets(info.Handle, writeCount, writes, copyCount, copies);
                }
            }
            finally
            {
                foreach (var item in attachedItems)
                {
                    Marshal.FreeHGlobal(item);
                }
            }
        }
    }
}
