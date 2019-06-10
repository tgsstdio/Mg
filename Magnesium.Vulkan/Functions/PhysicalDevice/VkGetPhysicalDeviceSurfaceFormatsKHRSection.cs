using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceFormatsKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceSurfaceFormatsKHR(IntPtr physicalDevice, UInt64 surface, ref UInt32 pSurfaceFormatCount, [In, Out] VkSurfaceFormatKHR[] pSurfaceFormats);

        public static MgResult GetPhysicalDeviceSurfaceFormatsKHR(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
        {
            var bSurface = (VkSurfaceKHR)surface;
            Debug.Assert(bSurface != null);

            var count = 0U;
            var first = vkGetPhysicalDeviceSurfaceFormatsKHR(info.Handle, bSurface.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pSurfaceFormats = null;
                return first;
            }

            var surfaceFormats = new VkSurfaceFormatKHR[count];
            var final = vkGetPhysicalDeviceSurfaceFormatsKHR(info.Handle, bSurface.Handle, ref count, surfaceFormats);

            pSurfaceFormats = new MgSurfaceFormatKHR[count];
            for (var i = 0; i < count; ++i)
            {
                pSurfaceFormats[i] = new MgSurfaceFormatKHR
                {
                    Format = (MgFormat)surfaceFormats[i].format,
                    ColorSpace = (MgColorSpaceKHR)surfaceFormats[i].colorSpace,
                };
            }

            return final;
        }
    }
}
