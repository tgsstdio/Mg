using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetDisplayPlaneCapabilities2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetDisplayPlaneCapabilities2KHR(IntPtr physicalDevice, VkDisplayPlaneInfo2KHR pDisplayPlaneInfo, [In, Out] VkDisplayPlaneCapabilities2KHR pCapabilities);

		public static MgResult GetDisplayPlaneCapabilities2KHR(VkPhysicalDeviceInfo info, MgDisplayPlaneInfo2KHR pDisplayPlaneInfo, out MgDisplayPlaneCapabilities2KHR pCapabilities)
		{
			// TODO: add implementation
		}
	}
}
