using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceSupportKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceSurfaceSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, UInt64 surface, ref VkBool32 pSupported);

        public static MgResult GetPhysicalDeviceSurfaceSupportKHR(VkPhysicalDeviceInfo info, UInt32 queueFamilyIndex, IMgSurfaceKHR surface, ref Boolean pSupported)
        {
            var bSurface = (VkSurfaceKHR)surface;
            Debug.Assert(bSurface != null);

            VkBool32 isSupported = default(VkBool32);
            var result = vkGetPhysicalDeviceSurfaceSupportKHR(info.Handle, queueFamilyIndex, bSurface.Handle, ref isSupported);
            pSupported = VkBool32.ConvertFrom(isSupported);
            return result;
        }
    }
}
