using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceCapabilities2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetPhysicalDeviceSurfaceCapabilities2KHR(IntPtr physicalDevice, VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, [In, Out] VkSurfaceCapabilities2KHR pSurfaceCapabilities);

		public static MgResult GetPhysicalDeviceSurfaceCapabilities2KHR(VkPhysicalDeviceInfo info, MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceCapabilities2KHR pSurfaceCapabilities)
		{
			// TODO: add implementation
		}
	}
}
