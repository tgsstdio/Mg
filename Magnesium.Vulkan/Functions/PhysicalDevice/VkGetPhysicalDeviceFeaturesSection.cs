using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceFeaturesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetPhysicalDeviceFeatures(IntPtr physicalDevice, VkPhysicalDeviceFeatures* pFeatures);

		public static void GetPhysicalDeviceFeatures(VkPhysicalDeviceInfo info, out MgPhysicalDeviceFeatures pFeatures)
		{
			// TODO: add implementation
		}
	}
}
