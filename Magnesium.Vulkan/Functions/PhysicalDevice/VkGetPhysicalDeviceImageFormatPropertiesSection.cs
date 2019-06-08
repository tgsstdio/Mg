using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceImageFormatPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetPhysicalDeviceImageFormatProperties(IntPtr physicalDevice, VkFormat format, VkImageType type, VkImageTiling tiling, VkImageUsageFlags usage, VkImageCreateFlags flags, [In, Out] VkImageFormatProperties pImageFormatProperties);

		public static MgResult GetPhysicalDeviceImageFormatProperties(VkPhysicalDeviceInfo info, MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
		{
			// TODO: add implementation
		}
	}
}
