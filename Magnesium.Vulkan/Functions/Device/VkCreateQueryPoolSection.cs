using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkCreateQueryPoolSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateQueryPool(IntPtr device, ref VkQueryPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pQueryPool);

        public static MgResult CreateQueryPool(VkDeviceInfo info, MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            var createInfo = new VkQueryPoolCreateInfo
            {
                sType = VkStructureType.StructureTypeQueryPoolCreateInfo,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
                queryType = (VkQueryType)pCreateInfo.QueryType,
                queryCount = (uint)pCreateInfo.QueryCount,
                pipelineStatistics = (VkQueryPipelineStatisticFlags)pCreateInfo.PipelineStatistics,
            };

            var internalHandle = 0UL;
            var result = vkCreateQueryPool(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
            queryPool = new VkQueryPool(internalHandle);

            return result;
        }
    }
}
