using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceDisplayPropertiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetPhysicalDeviceDisplayPropertiesKHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayPropertiesKHR[] pProperties);

		public static MgResult GetPhysicalDeviceDisplayPropertiesKHR(VkPhysicalDeviceInfo info, out MgDisplayPropertiesKHR[] pProperties)
		{
			// TODO: add implementation
		}
	}
}
