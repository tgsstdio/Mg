using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceExternalBufferPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceExternalBufferProperties(IntPtr physicalDevice, [In, Out] VkPhysicalDeviceExternalBufferInfo pExternalBufferInfo, [In, Out] VkExternalBufferProperties pExternalBufferProperties);

		public static void GetPhysicalDeviceExternalBufferProperties(VkPhysicalDeviceInfo info, MgPhysicalDeviceExternalBufferInfo pExternalBufferInfo, out MgExternalBufferProperties pExternalBufferProperties)
		{
			// TODO: add implementation
		}
	}
}
