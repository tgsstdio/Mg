using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceGeneratedCommandsPropertiesNVXSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX(IntPtr physicalDevice, VkDeviceGeneratedCommandsFeaturesNVX* pFeatures, VkDeviceGeneratedCommandsLimitsNVX* pLimits);

		public static void GetPhysicalDeviceGeneratedCommandsPropertiesNVX(VkPhysicalDeviceInfo info, MgDeviceGeneratedCommandsFeaturesNVX pFeatures, out MgDeviceGeneratedCommandsLimitsNVX pLimits)
		{
			// TODO: add implementation
		}
	}
}
