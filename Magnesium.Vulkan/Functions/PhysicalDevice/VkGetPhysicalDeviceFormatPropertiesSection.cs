using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceFormatPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceFormatProperties(IntPtr physicalDevice, MgFormat format, [In, Out] VkFormatProperties pFormatProperties);

		public static void GetPhysicalDeviceFormatProperties(VkPhysicalDeviceInfo info, MgFormat format, out MgFormatProperties pFormatProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
