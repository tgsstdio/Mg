using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSparseImageFormatPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceSparseImageFormatProperties(IntPtr physicalDevice, MgFormat format, VkImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, ref UInt32 pPropertyCount, [In, Out] VkSparseImageFormatProperties[] pProperties);

		public static void GetPhysicalDeviceSparseImageFormatProperties(VkPhysicalDeviceInfo info, MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
