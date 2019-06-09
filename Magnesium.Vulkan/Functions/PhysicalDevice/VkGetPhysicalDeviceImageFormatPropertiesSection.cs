using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceImageFormatPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceImageFormatProperties(IntPtr physicalDevice, MgFormat format, VkImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, ref VkImageFormatProperties pImageFormatProperties);

        public static MgResult GetPhysicalDeviceImageFormatProperties(VkPhysicalDeviceInfo info, MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
        {
            var bType = (VkImageType)type;

            var properties = default(VkImageFormatProperties);
            var result = vkGetPhysicalDeviceImageFormatProperties
            (
                info.Handle,
                format,
                bType,
                tiling,
                usage,
                flags,
                ref properties
           );

            pImageFormatProperties = new MgImageFormatProperties
            {
                MaxExtent = properties.maxExtent,
                MaxMipLevels = properties.maxMipLevels,
                MaxArrayLayers = properties.maxArrayLayers,
                SampleCounts = properties.sampleCounts,
                MaxResourceSize = properties.maxResourceSize,
            };
            return result;
        }
    }
}
