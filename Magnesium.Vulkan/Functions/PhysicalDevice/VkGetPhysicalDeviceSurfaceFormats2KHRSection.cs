using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceFormats2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceSurfaceFormats2KHR(IntPtr physicalDevice, ref VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, ref UInt32 pSurfaceFormatCount, [In, Out] VkSurfaceFormat2KHR[] pSurfaceFormats);

        public static MgResult GetPhysicalDeviceSurfaceFormats2KHR(VkPhysicalDeviceInfo info, MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceFormat2KHR[] pSurfaceFormats)
        {
            var bSurface = (VkSurfaceKHR)pSurfaceInfo.Surface;
            Debug.Assert(bSurface != null);

            var bSurfaceInfo = new VkPhysicalDeviceSurfaceInfo2KHR
            {
                sType = VkStructureType.StructureTypePhysicalDeviceSurfaceInfo2Khr,
                // TODO: extension
                pNext = IntPtr.Zero,
                surface = bSurface.Handle,
            };

            var count = 0U;
            var first = vkGetPhysicalDeviceSurfaceFormats2KHR(info.Handle, ref bSurfaceInfo, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pSurfaceFormats = null;
                return first;
            }

            var surfaceFormats = new VkSurfaceFormat2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                surfaceFormats[i] = new VkSurfaceFormat2KHR
                {
                    sType = VkStructureType.StructureTypeSurfaceFormat2Khr,
                    pNext = IntPtr.Zero,
                };
            }

            var final = vkGetPhysicalDeviceSurfaceFormats2KHR(info.Handle, ref bSurfaceInfo, ref count, surfaceFormats);

            pSurfaceFormats = new MgSurfaceFormat2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                pSurfaceFormats[i] = new MgSurfaceFormat2KHR
                {
                    SurfaceFormat = TranslateSurfaceFormatKHR(ref surfaceFormats[i].surfaceFormat),
                };
            }

            return final;
        }

        private static MgSurfaceFormatKHR TranslateSurfaceFormatKHR(ref VkSurfaceFormatKHR src)
        {
            return new MgSurfaceFormatKHR
            {
                Format = (MgFormat)src.format,
                ColorSpace = (MgColorSpaceKHR)src.colorSpace,
            };
        }
    }
}
