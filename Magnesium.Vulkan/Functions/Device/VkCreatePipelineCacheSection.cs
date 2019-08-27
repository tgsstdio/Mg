using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkCreatePipelineCacheSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreatePipelineCache(IntPtr device, ref VkPipelineCacheCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pPipelineCache);

        public static MgResult CreatePipelineCache(VkDeviceInfo info, MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            var createInfo = new VkPipelineCacheCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineCacheCreateInfo,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
                initialDataSize = pCreateInfo.InitialDataSize,
                pInitialData = pCreateInfo.InitialData,
            };

            ulong internalHandle = 0;
            var result = vkCreatePipelineCache(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
            pPipelineCache = new VkPipelineCache(internalHandle);
            return result;
        }
    }
}
