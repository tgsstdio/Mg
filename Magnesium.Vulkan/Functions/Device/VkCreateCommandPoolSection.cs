using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkCreateCommandPoolSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateCommandPool(IntPtr device, ref VkCommandPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pCommandPool);

        public static MgResult CreateCommandPool(VkDeviceInfo info, MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            ulong internalHandle = 0UL;
            var createInfo = new VkCommandPoolCreateInfo
            {
                sType = VkStructureType.StructureTypeCommandPoolCreateInfo,
                pNext = IntPtr.Zero,
                flags = (VkCommandPoolCreateFlags)pCreateInfo.Flags,
                queueFamilyIndex = pCreateInfo.QueueFamilyIndex,
            };
            var result = vkCreateCommandPool(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
            pCommandPool = new VkCommandPool(internalHandle);
            return result;
        }
    }
}
