using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceMemoryPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceMemoryProperties(IntPtr physicalDevice, [In, Out] VkPhysicalDeviceMemoryProperties pMemoryProperties);

		public static void GetPhysicalDeviceMemoryProperties(VkPhysicalDeviceInfo info, out MgPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			// TODO: add implementation
		}
	}
}
