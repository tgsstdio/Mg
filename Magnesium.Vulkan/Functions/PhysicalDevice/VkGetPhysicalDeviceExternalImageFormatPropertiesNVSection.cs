using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceExternalImageFormatPropertiesNVSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceExternalImageFormatPropertiesNV(IntPtr physicalDevice, VkFormat format, VkImageType type, VkImageTiling tiling, VkImageUsageFlags usage, VkImageCreateFlags flags, VkExternalMemoryHandleTypeFlagsNV externalHandleType, [In, Out] VkExternalImageFormatPropertiesNV pExternalImageFormatProperties);

		public static MgResult GetPhysicalDeviceExternalImageFormatPropertiesNV(VkPhysicalDeviceInfo info, MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, UInt32 externalHandleType, out MgExternalImageFormatPropertiesNV pExternalImageFormatProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
