using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetDisplayPlaneCapabilitiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetDisplayPlaneCapabilitiesKHR(IntPtr physicalDevice, UInt64 mode, UInt32 planeIndex, ref VkDisplayPlaneCapabilitiesKHR pCapabilities);

        public static MgResult GetDisplayPlaneCapabilitiesKHR(VkPhysicalDeviceInfo info, IMgDisplayModeKHR mode, UInt32 planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
        {
            var bMode = (VkDisplayModeKHR)mode;
            Debug.Assert(bMode != null);

            var capabilities = default(VkDisplayPlaneCapabilitiesKHR);
            var result = vkGetDisplayPlaneCapabilitiesKHR(info.Handle, bMode.Handle, planeIndex, ref capabilities);

            pCapabilities = new MgDisplayPlaneCapabilitiesKHR
            {
                SupportedAlpha = (MgDisplayPlaneAlphaFlagBitsKHR)capabilities.supportedAlpha,
                MinSrcPosition = capabilities.minSrcPosition,
                MaxSrcPosition = capabilities.maxSrcPosition,
                MinSrcExtent = capabilities.minSrcExtent,
                MaxSrcExtent = capabilities.maxSrcExtent,
                MinDstPosition = capabilities.minDstPosition,
                MaxDstPosition = capabilities.maxDstPosition,
                MinDstExtent = capabilities.minDstExtent,
                MaxDstExtent = capabilities.maxDstExtent,
            };
            return result;
        }
    }
}
