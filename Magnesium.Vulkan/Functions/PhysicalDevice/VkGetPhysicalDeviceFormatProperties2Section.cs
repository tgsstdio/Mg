using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceFormatProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceFormatProperties2(IntPtr physicalDevice, MgFormat format, [In, Out] VkFormatProperties2 pFormatProperties);

		public static void GetPhysicalDeviceFormatProperties2(VkPhysicalDeviceInfo info, MgFormat format, out MgFormatProperties2 pFormatProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
