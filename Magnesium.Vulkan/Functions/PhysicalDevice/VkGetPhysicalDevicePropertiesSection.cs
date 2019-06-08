using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDevicePropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceProperties(IntPtr physicalDevice, [In, Out] VkPhysicalDeviceProperties pProperties);

		public static void GetPhysicalDeviceProperties(VkPhysicalDeviceInfo info, out MgPhysicalDeviceProperties pProperties)
		{
			// TODO: add implementation
		}
	}
}
