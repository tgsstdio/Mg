using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceCapabilities2EXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe MgResult vkGetPhysicalDeviceSurfaceCapabilities2EXT(IntPtr physicalDevice, UInt64 surface, ref VkSurfaceCapabilities2EXT pSurfaceCapabilities);

        public static MgResult GetPhysicalDeviceSurfaceCapabilities2EXT(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, out MgSurfaceCapabilities2EXT pSurfaceCapabilities)
		{
            var bSurface = (VkSurfaceKHR)surface;
            Debug.Assert(bSurface != null);

            var pCreateInfo = new VkSurfaceCapabilities2EXT
            {
                sType = VkStructureType.StructureTypeSurfaceCapabilities2Ext,
                // TODO: extension
                pNext = IntPtr.Zero,
            };

            var result = vkGetPhysicalDeviceSurfaceCapabilities2EXT(info.Handle, bSurface.Handle, ref pCreateInfo);

            pSurfaceCapabilities = new MgSurfaceCapabilities2EXT
            {
                MinImageCount = pCreateInfo.minImageCount,
                MaxImageCount = pCreateInfo.maxImageCount,
                CurrentExtent = pCreateInfo.currentExtent,
                MinImageExtent = pCreateInfo.minImageExtent,
                MaxImageExtent = pCreateInfo.maxImageExtent,
                MaxImageArrayLayers = pCreateInfo.maxImageArrayLayers,
                SupportedTransforms = (MgSurfaceTransformFlagBitsKHR)pCreateInfo.supportedTransforms,
                CurrentTransform = (MgSurfaceTransformFlagBitsKHR)pCreateInfo.currentTransform,
                SupportedCompositeAlpha = (MgCompositeAlphaFlagBitsKHR)pCreateInfo.supportedCompositeAlpha,
                SupportedUsageFlags = (MgImageUsageFlagBits)pCreateInfo.supportedUsageFlags,
                SupportedSurfaceCounters = pCreateInfo.supportedSurfaceCounters,
            };

            return result;
        }
	}
}
