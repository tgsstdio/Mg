using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkAllocateCommandBuffersSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe MgResult vkAllocateCommandBuffers(IntPtr device, VkCommandBufferAllocateInfo* pAllocateInfo, IntPtr* pCommandBuffers);

        public static MgResult AllocateCommandBuffers(VkDeviceInfo info, MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bCommandPool = (VkCommandPool)pAllocateInfo.CommandPool;
            Debug.Assert(bCommandPool != null);

            unsafe
            {
                var arraySize = (int)pAllocateInfo.CommandBufferCount;

                var pBufferHandle = stackalloc IntPtr[arraySize];

                var allocateInfo = stackalloc VkCommandBufferAllocateInfo[1];

                allocateInfo[0] = new VkCommandBufferAllocateInfo
                {
                    sType = VkStructureType.StructureTypeCommandBufferAllocateInfo,
                    pNext = IntPtr.Zero,
                    commandBufferCount = pAllocateInfo.CommandBufferCount,
                    commandPool = bCommandPool.Handle,
                    level = (VkCommandBufferLevel)pAllocateInfo.Level,
                };

                var result = vkAllocateCommandBuffers(info.Handle, allocateInfo, pBufferHandle);

                for (var i = 0; i < arraySize; ++i)
                {
                    pCommandBuffers[i] = new VkCommandBuffer(pBufferHandle[i]);
                }
                return result;
            }
        }
    }
}
