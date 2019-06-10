using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceExternalImageFormatPropertiesNVSection
	{
        [DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceExternalImageFormatPropertiesNV(IntPtr physicalDevice, MgFormat format, VkImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, UInt32 externalHandleType, ref VkExternalImageFormatPropertiesNV pExternalImageFormatProperties);

        public static MgResult GetPhysicalDeviceExternalImageFormatPropertiesNV(VkPhysicalDeviceInfo info, MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, UInt32 externalHandleType, out MgExternalImageFormatPropertiesNV pExternalImageFormatProperties)
        {
            var bType = (VkImageType)type;

            var properties = default(VkExternalImageFormatPropertiesNV);

            var result = vkGetPhysicalDeviceExternalImageFormatPropertiesNV(
                info.Handle,
                format,
                bType,
                tiling,
                usage,
                flags,
                externalHandleType,
                ref properties);

            pExternalImageFormatProperties = new MgExternalImageFormatPropertiesNV
            {
                ImageFormatProperties = new MgImageFormatProperties
                {
                    MaxExtent = properties.imageFormatProperties.maxExtent,
                    MaxArrayLayers = properties.imageFormatProperties.maxArrayLayers,
                    MaxMipLevels = properties.imageFormatProperties.maxMipLevels,
                    MaxResourceSize = properties.imageFormatProperties.maxResourceSize,
                    SampleCounts = (MgSampleCountFlagBits)properties.imageFormatProperties.sampleCounts,
                },
                CompatibleHandleTypes = properties.compatibleHandleTypes,
                ExportFromImportedHandleTypes = properties.exportFromImportedHandleTypes,
                ExternalMemoryFeatures = properties.externalMemoryFeatures,
            };

            return result;
        }
    }
}
