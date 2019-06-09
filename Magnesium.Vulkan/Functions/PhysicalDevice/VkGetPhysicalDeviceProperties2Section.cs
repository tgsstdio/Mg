using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceProperties2(IntPtr physicalDevice, [In, Out] VkPhysicalDeviceProperties2 pProperties);

		public static void GetPhysicalDeviceProperties2(VkPhysicalDeviceInfo info, out MgPhysicalDeviceProperties2 pProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
