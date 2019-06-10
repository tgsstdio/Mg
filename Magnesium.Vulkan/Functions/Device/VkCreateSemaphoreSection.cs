using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateSemaphoreSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateSemaphore(IntPtr device, ref VkSemaphoreCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSemaphore);

        public static MgResult CreateSemaphore(VkDeviceInfo info, MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var createInfo = new VkSemaphoreCreateInfo
            {
                sType = VkStructureType.StructureTypeSemaphoreCreateInfo,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
            };

            var internalHandle = 0UL;
            var result = vkCreateSemaphore(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
            pSemaphore = new VkSemaphore(internalHandle);

            return result;
        }
    }
}
