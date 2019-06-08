using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceExternalFencePropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceExternalFenceProperties(IntPtr physicalDevice, VkPhysicalDeviceExternalFenceInfo pExternalFenceInfo, [In, Out] VkExternalFenceProperties pExternalFenceProperties);

		public static void GetPhysicalDeviceExternalFenceProperties(VkPhysicalDeviceInfo info, MgPhysicalDeviceExternalFenceInfo pExternalFenceInfo, out MgExternalFenceProperties pExternalFenceProperties)
		{
			// TODO: add implementation
		}
	}
}
