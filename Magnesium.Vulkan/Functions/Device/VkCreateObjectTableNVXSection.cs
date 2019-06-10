using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateObjectTableNVXSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateObjectTableNVX(IntPtr device, ref VkObjectTableCreateInfoNVX pCreateInfo, IntPtr pAllocator, ref UInt64 pObjectTable);

        public static MgResult CreateObjectTableNVX(VkDeviceInfo info, MgObjectTableCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgObjectTableNVX pObjectTable)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var pObjectEntryTypes = IntPtr.Zero;
            var pObjectEntryCounts = IntPtr.Zero;
            var pObjectEntryUsageFlags = IntPtr.Zero;

            try
            {
                var objectCount = (UInt32)pCreateInfo.Entries.Length;

                if (objectCount > 0)
                {
                    var counts = new UInt32[objectCount];
                    var types = new UInt32[objectCount];
                    var flags = new UInt32[objectCount];

                    for (var i = 0; i < objectCount; i += 1)
                    {
                        var current = pCreateInfo.Entries[i];

                        counts[i] = current.ObjectEntryCount;
                        types[i] = (UInt32)current.ObjectEntryType;
                        flags[i] = (UInt32)current.UsageFlag;
                    }

                    pObjectEntryCounts = VkInteropsUtility.AllocateUInt32Array(counts);
                    pObjectEntryTypes = VkInteropsUtility.AllocateUInt32Array(types);
                    pObjectEntryUsageFlags = VkInteropsUtility.AllocateUInt32Array(flags);
                }

                var bCreateInfo = new VkObjectTableCreateInfoNVX
                {
                    sType = VkStructureType.StructureTypeObjectTableCreateInfoNvx,
                    pNext = IntPtr.Zero,
                    objectCount = objectCount,
                    pObjectEntryTypes = pObjectEntryTypes,
                    pObjectEntryCounts = pObjectEntryCounts,
                    pObjectEntryUsageFlags = pObjectEntryUsageFlags,
                    maxPipelineLayouts = pCreateInfo.MaxPipelineLayouts,
                    maxSampledImagesPerDescriptor = pCreateInfo.MaxSampledImagesPerDescriptor,
                    maxStorageBuffersPerDescriptor = pCreateInfo.MaxStorageBuffersPerDescriptor,
                    maxStorageImagesPerDescriptor = pCreateInfo.MaxStorageImagesPerDescriptor,
                    maxUniformBuffersPerDescriptor = pCreateInfo.MaxUniformBuffersPerDescriptor,
                };

                ulong handle = 0UL;
                var result = vkCreateObjectTableNVX(info.Handle, ref bCreateInfo, allocatorPtr, ref handle);

                pObjectTable = new VkObjectTableNVX(handle);
                return result;
            }
            finally
            {
                if (pObjectEntryTypes != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pObjectEntryTypes);
                }

                if (pObjectEntryCounts != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pObjectEntryCounts);
                }

                if (pObjectEntryUsageFlags != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pObjectEntryUsageFlags);
                }
            }
        }
    }
}
