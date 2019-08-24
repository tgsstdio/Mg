using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkCreateWin32SurfaceKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static MgResult vkCreateWin32SurfaceKHR(IntPtr instance, ref VkWin32SurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

        public static MgResult CreateWin32SurfaceKHR(VkInstanceInfo info, MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
        {
            Debug.Assert(!info.IsDisposed);

            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);

            var createInfo = new VkWin32SurfaceCreateInfoKHR
            {
                sType = VkStructureType.StructureTypeWin32SurfaceCreateInfoKhr,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
                hinstance = pCreateInfo.Hinstance,
                hwnd = pCreateInfo.Hwnd,
            };

            // TODO : MIGHT NEED GetInstanceProcAddr INSTEAD

            var surfaceHandle = 0UL;
            var result = vkCreateWin32SurfaceKHR(info.Handle, ref createInfo, allocatorHandle, ref surfaceHandle);
            pSurface = new VkSurfaceKHR(surfaceHandle);

            return result;
        }
    }
}
