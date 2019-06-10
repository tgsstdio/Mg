using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceCapabilitiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceSurfaceCapabilitiesKHR(IntPtr physicalDevice, UInt64 surface, ref VkSurfaceCapabilitiesKHR pSurfaceCapabilities);

        public static MgResult GetPhysicalDeviceSurfaceCapabilitiesKHR(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
        {
            var bSurface = (VkSurfaceKHR)surface;
            Debug.Assert(bSurface != null);

            var pCreateInfo = default(VkSurfaceCapabilitiesKHR);
            var result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(info.Handle, bSurface.Handle, ref pCreateInfo);
            pSurfaceCapabilities = TranslateSurfaceCapabilities(ref pCreateInfo);

            return result;
        }

        private static MgSurfaceCapabilitiesKHR TranslateSurfaceCapabilities(ref VkSurfaceCapabilitiesKHR src)
        {
            return new MgSurfaceCapabilitiesKHR
            {
                MinImageCount = src.minImageCount,
                MaxImageCount = src.maxImageCount,
                CurrentExtent = src.currentExtent,
                MinImageExtent = src.minImageExtent,
                MaxImageExtent = src.maxImageExtent,
                MaxImageArrayLayers = src.maxImageArrayLayers,
                SupportedTransforms = (MgSurfaceTransformFlagBitsKHR)src.supportedTransforms,
                CurrentTransform = (MgSurfaceTransformFlagBitsKHR)src.currentTransform,
                SupportedCompositeAlpha = (MgCompositeAlphaFlagBitsKHR)src.supportedCompositeAlpha,
                SupportedUsageFlags = (MgImageUsageFlagBits)src.supportedUsageFlags,
            };
        }
    }
}
