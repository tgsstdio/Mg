using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceCapabilitiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfaceCapabilitiesKHR(IntPtr physicalDevice, UInt64 surface, [In, Out] VkSurfaceCapabilitiesKHR pSurfaceCapabilities);

		public static MgResult GetPhysicalDeviceSurfaceCapabilitiesKHR(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
		{
			// TODO: add implementation
			throw new NotImplementedException();
		}
	}
}
