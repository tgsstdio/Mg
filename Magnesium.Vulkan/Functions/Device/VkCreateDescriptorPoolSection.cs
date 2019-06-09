using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateDescriptorPoolSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateDescriptorPool(IntPtr device, ref VkDescriptorPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pDescriptorPool);

        public static MgResult CreateDescriptorPool(VkDeviceInfo info, MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var pPoolSizes = IntPtr.Zero;
            var poolSizeCount = 0U;

            try
            {
                if (pCreateInfo.PoolSizes != null)
                {
                    poolSizeCount = (UInt32)pCreateInfo.PoolSizes.Length;
                    if (poolSizeCount > 0)
                    {
                        pPoolSizes = VkInteropsUtility.AllocateHGlobalArray(
                            pCreateInfo.PoolSizes,
                            (current) =>
                            {
                                return new VkDescriptorPoolSize
                                {
                                    type = current.Type,
                                    descriptorCount = current.DescriptorCount,
                                };
                            });
                    }
                }
                var createInfo = new VkDescriptorPoolCreateInfo
                {
                    sType = VkStructureType.StructureTypeDescriptorPoolCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = (VkDescriptorPoolCreateFlags)pCreateInfo.Flags,
                    maxSets = pCreateInfo.MaxSets,
                    poolSizeCount = poolSizeCount,
                    pPoolSizes = pPoolSizes,
                };

                var internalHandle = 0UL;
                var result = vkCreateDescriptorPool(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pDescriptorPool = new VkDescriptorPool(internalHandle);
                return result;
            }
            finally
            {
                if (pPoolSizes != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pPoolSizes);
                }
            }
        }
    }
}
