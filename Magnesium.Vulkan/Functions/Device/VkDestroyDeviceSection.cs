using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkDestroyDeviceSection
	{
        [DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static void vkDestroyDevice(IntPtr device, IntPtr pAllocator);

        public static void DestroyDevice(VkDeviceInfo info, IMgAllocationCallbacks allocator)
		{
            if (info.IsDisposed)
                return;

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            vkDestroyDevice(info.Handle, allocatorPtr);

            info.Handle = IntPtr.Zero;
            info.IsDisposed = true;
        }
	}
}
