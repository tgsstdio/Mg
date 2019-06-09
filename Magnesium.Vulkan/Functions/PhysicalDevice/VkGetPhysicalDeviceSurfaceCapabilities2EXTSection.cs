using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceCapabilities2EXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfaceCapabilities2EXT(IntPtr physicalDevice, UInt64 surface, [In, Out] VkSurfaceCapabilities2EXT pSurfaceCapabilities);

		public static MgResult GetPhysicalDeviceSurfaceCapabilities2EXT(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, out MgSurfaceCapabilities2EXT pSurfaceCapabilities)
		{
			// TODO: add implementation
		}
	}
}
