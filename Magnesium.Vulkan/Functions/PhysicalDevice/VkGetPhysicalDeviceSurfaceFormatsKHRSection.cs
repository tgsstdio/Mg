using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceFormatsKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkGetPhysicalDeviceSurfaceFormatsKHR(IntPtr physicalDevice, UInt64 surface, UInt32* pSurfaceFormatCount, VkSurfaceFormatKHR* pSurfaceFormats);

		public static MgResult GetPhysicalDeviceSurfaceFormatsKHR(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
		{
			// TODO: add implementation
			throw new NotImplementedException();
		}
	}
}
