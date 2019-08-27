using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkAllocateMemorySection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe MgResult vkAllocateMemory(IntPtr device, VkMemoryAllocateInfo* pAllocateInfo, IntPtr pAllocator, UInt64* pMemory);

        public static MgResult AllocateMemory(VkDeviceInfo info, MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
		{
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            unsafe
            {
                var allocateInfo = stackalloc VkMemoryAllocateInfo[1];

                allocateInfo[0] = new VkMemoryAllocateInfo
                {
                    sType = VkStructureType.StructureTypeMemoryAllocateInfo,
                    pNext = IntPtr.Zero,
                    allocationSize = pAllocateInfo.AllocationSize,
                    memoryTypeIndex = pAllocateInfo.MemoryTypeIndex,
                };

                var memoryHandle = stackalloc ulong[1];
                var result = vkAllocateMemory(info.Handle, allocateInfo, allocatorPtr, memoryHandle);

                pMemory = new VkDeviceMemory(memoryHandle[0]);
                return result;
            }
        }
	}
}
