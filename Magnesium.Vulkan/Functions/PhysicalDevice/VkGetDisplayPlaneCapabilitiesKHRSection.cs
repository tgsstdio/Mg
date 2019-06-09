using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetDisplayPlaneCapabilitiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetDisplayPlaneCapabilitiesKHR(IntPtr physicalDevice, UInt64 mode, UInt32 planeIndex, [In, Out] VkDisplayPlaneCapabilitiesKHR pCapabilities);

		public static MgResult GetDisplayPlaneCapabilitiesKHR(VkPhysicalDeviceInfo info, IMgDisplayModeKHR mode, UInt32 planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
		{
			// TODO: add implementation
		}
	}
}
