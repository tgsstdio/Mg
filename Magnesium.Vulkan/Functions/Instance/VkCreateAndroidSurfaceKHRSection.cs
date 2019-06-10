using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkCreateAndroidSurfaceKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static MgResult vkCreateAndroidSurfaceKHR(IntPtr instance, ref VkAndroidSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

        public static MgResult CreateAndroidSurfaceKHR(VkInstanceInfo info, MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
        {
            Debug.Assert(!info.IsDisposed);

            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);

            // TODO : MIGHT NEED GetInstanceProcAddr INSTEAD
            var createInfo = new VkAndroidSurfaceCreateInfoKHR
            {
                sType = VkStructureType.StructureTypeAndroidSurfaceCreateInfoKhr,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
                window = pCreateInfo.Window,
            };

            var surfaceHandle = 0UL;
            var result = vkCreateAndroidSurfaceKHR(info.Handle, ref createInfo, allocatorHandle, ref surfaceHandle);
            pSurface = new VkSurfaceKHR(surfaceHandle);

            return result;
        }
    }
}
