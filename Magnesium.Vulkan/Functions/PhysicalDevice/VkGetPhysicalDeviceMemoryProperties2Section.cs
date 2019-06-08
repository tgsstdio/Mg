using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceMemoryProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceMemoryProperties2(IntPtr physicalDevice, [In, Out] VkPhysicalDeviceMemoryProperties2 pMemoryProperties);

		public static void GetPhysicalDeviceMemoryProperties2(VkPhysicalDeviceInfo info, out MgPhysicalDeviceMemoryProperties2 pMemoryProperties)
		{
			// TODO: add implementation
		}
	}
}
