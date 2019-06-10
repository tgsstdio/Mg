using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfacePresentModesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceSurfacePresentModesKHR(IntPtr physicalDevice, UInt64 surface, ref UInt32 pPresentModeCount, [In, Out] VkPresentModeKhr[] pPresentModes);

        public static MgResult GetPhysicalDeviceSurfacePresentModesKHR(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
        {
            var bSurface = (VkSurfaceKHR)surface;
            Debug.Assert(bSurface != null);

            var count = 0U;
            var first = vkGetPhysicalDeviceSurfacePresentModesKHR(info.Handle, bSurface.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pPresentModes = null;
                return first;
            }

            var modes = new VkPresentModeKhr[count];
            var final = vkGetPhysicalDeviceSurfacePresentModesKHR(info.Handle, bSurface.Handle, ref count, modes);

            pPresentModes = new MgPresentModeKHR[count];
            for (var i = 0; i < count; ++i)
            {
                pPresentModes[i] = (MgPresentModeKHR)modes[i];
            }

            return final;
        }
    }
}
