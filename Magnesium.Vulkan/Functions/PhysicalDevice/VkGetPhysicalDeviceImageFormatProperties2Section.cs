using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceImageFormatProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceImageFormatProperties2(IntPtr physicalDevice, [In, Out] VkPhysicalDeviceImageFormatInfo2 pImageFormatInfo, [In, Out] VkImageFormatProperties2 pImageFormatProperties);

		public static MgResult GetPhysicalDeviceImageFormatProperties2(VkPhysicalDeviceInfo info, MgPhysicalDeviceImageFormatInfo2 pImageFormatInfo, MgImageFormatProperties2 pImageFormatProperties)
		{
			// TODO: add implementation
			throw new NotImplementedException();
		}
	}
}
