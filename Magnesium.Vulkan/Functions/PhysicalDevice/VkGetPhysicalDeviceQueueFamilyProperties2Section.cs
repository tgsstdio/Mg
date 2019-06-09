using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceQueueFamilyProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceQueueFamilyProperties2(IntPtr physicalDevice, ref UInt32 pQueueFamilyPropertyCount, [In, Out] VkQueueFamilyProperties2[] pQueueFamilyProperties);

		public static void GetPhysicalDeviceQueueFamilyProperties2(VkPhysicalDeviceInfo info, out MgQueueFamilyProperties2[] pQueueFamilyProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
