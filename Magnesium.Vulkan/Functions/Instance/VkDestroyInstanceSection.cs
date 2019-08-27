using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public static class VkDestroyInstanceSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkDestroyInstance(IntPtr instance, IntPtr pAllocator);

        public static void DestroyInstance(VkInstanceInfo info, IMgAllocationCallbacks allocator)
		{
            if (!info.IsDisposed)
                return;

            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);

            vkDestroyInstance(info.Handle, allocatorHandle);

            info.Handle = IntPtr.Zero;
            info.IsDisposed = true;
        }
	}
}
