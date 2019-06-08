using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceDisplayPlanePropertiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkGetPhysicalDeviceDisplayPlanePropertiesKHR(IntPtr physicalDevice, UInt32* pPropertyCount, VkDisplayPlanePropertiesKHR* pProperties);

		public static MgResult GetPhysicalDeviceDisplayPlanePropertiesKHR(VkPhysicalDeviceInfo info, out MgDisplayPlanePropertiesKHR[] pProperties)
		{
			// TODO: add implementation
		}
	}
}
