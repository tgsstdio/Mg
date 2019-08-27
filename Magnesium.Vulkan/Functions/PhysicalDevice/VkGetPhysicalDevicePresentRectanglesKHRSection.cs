using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDevicePresentRectanglesKHRSection
	{
        [DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe MgResult vkGetPhysicalDevicePresentRectanglesKHR(IntPtr physicalDevice, UInt64 surface, UInt32 pRectCount, MgRect2D* pRects);

        public static MgResult GetPhysicalDevicePresentRectanglesKHR(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, MgRect2D[] pRects)
        {
            var hRects = GCHandle.Alloc(pRects, GCHandleType.Pinned);

            try
            {
                var bSurface = (VkSurfaceKHR)surface;

                unsafe
                {
                    var count = (UInt32)pRects.Length;
                    var pinnedObject = hRects.AddrOfPinnedObject();

                    var bRects = (MgRect2D*)pinnedObject.ToPointer();

                    return vkGetPhysicalDevicePresentRectanglesKHR(info.Handle, bSurface.Handle, count, bRects);
                }
            }
            finally
            {
                hRects.Free();
            }
        }
    }
}
