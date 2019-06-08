using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceFormats2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkGetPhysicalDeviceSurfaceFormats2KHR(IntPtr physicalDevice, VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, UInt32* pSurfaceFormatCount, VkSurfaceFormat2KHR* pSurfaceFormats);

		public static MgResult GetPhysicalDeviceSurfaceFormats2KHR(VkPhysicalDeviceInfo info, MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceFormat2KHR[] pSurfaceFormats)
		{
			// TODO: add implementation
		}
	}
}
