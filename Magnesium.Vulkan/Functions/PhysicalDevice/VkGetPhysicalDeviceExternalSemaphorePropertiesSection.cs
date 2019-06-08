using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceExternalSemaphorePropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceExternalSemaphoreProperties(IntPtr physicalDevice, VkPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, [In, Out] VkExternalSemaphoreProperties pExternalSemaphoreProperties);

		public static void GetPhysicalDeviceExternalSemaphoreProperties(VkPhysicalDeviceInfo info, MgPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, out MgExternalSemaphoreProperties pExternalSemaphoreProperties)
		{
			// TODO: add implementation
		}
	}
}
