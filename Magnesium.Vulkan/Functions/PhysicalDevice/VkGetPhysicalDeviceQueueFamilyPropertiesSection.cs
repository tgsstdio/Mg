using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceQueueFamilyPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceQueueFamilyProperties(IntPtr physicalDevice, ref UInt32 pQueueFamilyPropertyCount, [In, Out] VkQueueFamilyProperties[] pQueueFamilyProperties);

		public static void GetPhysicalDeviceQueueFamilyProperties(VkPhysicalDeviceInfo info, out MgQueueFamilyProperties[] pQueueFamilyProperties)
		{
			// TODO: add implementation
		}
	}
}
