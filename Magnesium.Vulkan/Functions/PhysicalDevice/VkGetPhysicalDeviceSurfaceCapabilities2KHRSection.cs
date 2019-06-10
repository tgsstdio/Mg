using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceCapabilities2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceSurfaceCapabilities2KHR(IntPtr physicalDevice, ref VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, ref VkSurfaceCapabilities2KHR pSurfaceCapabilities);

        public static MgResult GetPhysicalDeviceSurfaceCapabilities2KHR(VkPhysicalDeviceInfo info, MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceCapabilities2KHR pSurfaceCapabilities)
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

            var output = new VkSurfaceCapabilities2KHR
            {
                sType = VkStructureType.StructureTypeSurfaceCapabilities2Khr,
                // TODO: extension
                pNext = IntPtr.Zero,
            };

            var result = vkGetPhysicalDeviceSurfaceCapabilities2KHR(info.Handle, ref bSurfaceInfo, ref output);

            pSurfaceCapabilities = new MgSurfaceCapabilities2KHR
            {
                SurfaceCapabilities = VkGetPhysicalDeviceSurfaceCapabilitiesKHRSection.TranslateSurfaceCapabilities(ref output.surfaceCapabilities),
            };

            return result;
        }
    }
}
