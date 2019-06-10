using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetDisplayPlaneCapabilities2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetDisplayPlaneCapabilities2KHR(IntPtr physicalDevice, ref VkDisplayPlaneInfo2KHR pDisplayPlaneInfo, ref VkDisplayPlaneCapabilities2KHR pCapabilities);

        public static MgResult GetDisplayPlaneCapabilities2KHR(VkPhysicalDeviceInfo info, MgDisplayPlaneInfo2KHR pDisplayPlaneInfo, out MgDisplayPlaneCapabilities2KHR pCapabilities)
        {
            var bMode = (VkDisplayModeKHR)pDisplayPlaneInfo.Mode;
            Debug.Assert(bMode != null);

            var bDisplayPlaneInfo = new VkDisplayPlaneInfo2KHR
            {
                sType = VkStructureType.StructureTypeDisplayPlaneInfo2Khr,
                pNext = IntPtr.Zero, // TODO: extension
                mode = bMode.Handle,
                planeIndex = pDisplayPlaneInfo.PlaneIndex,
            };

            var output = new VkDisplayPlaneCapabilities2KHR
            {
                sType = VkStructureType.StructureTypeDisplayPlaneCapabilities2Khr,
                // TODO: extension
                pNext = IntPtr.Zero,
            };

            var result = vkGetDisplayPlaneCapabilities2KHR(info.Handle, ref bDisplayPlaneInfo, ref output);

            var caps = output.capabilities;
            pCapabilities = new MgDisplayPlaneCapabilities2KHR
            {
                Capabilities = new MgDisplayPlaneCapabilitiesKHR
                {
                    SupportedAlpha = (MgDisplayPlaneAlphaFlagBitsKHR)caps.supportedAlpha,
                    MinSrcPosition = caps.minSrcPosition,
                    MaxSrcPosition = caps.maxSrcPosition,
                    MinSrcExtent = caps.minSrcExtent,
                    MaxSrcExtent = caps.maxSrcExtent,
                    MinDstPosition = caps.minDstPosition,
                    MaxDstPosition = caps.maxDstPosition,
                    MinDstExtent = caps.minDstExtent,
                    MaxDstExtent = caps.maxDstExtent,
                }
            };
            return result;
        }
    }
}
