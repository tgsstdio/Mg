using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceSparseImageFormatPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceSparseImageFormatProperties(IntPtr physicalDevice, MgFormat format, VkImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, ref UInt32 pPropertyCount, [In, Out] VkSparseImageFormatProperties[] pProperties);

        public static void GetPhysicalDeviceSparseImageFormatProperties(VkPhysicalDeviceInfo info, MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
        {
            uint count = 0;

            var bType = (VkImageType)type;
            var bSamples = samples;

            vkGetPhysicalDeviceSparseImageFormatProperties
            (
                info.Handle,
                format,
                bType,
                bSamples,
                usage,
                tiling,
                ref count,
                null
               );

            if (count == 0)
            {
                pProperties = new MgSparseImageFormatProperties[0];
                return;
            }

            var formatProperties = new VkSparseImageFormatProperties[count];
            vkGetPhysicalDeviceSparseImageFormatProperties
            (
                info.Handle,
                format,
                bType,
                bSamples,
                usage,
                tiling,
                ref count,
                formatProperties
               );

            pProperties = new MgSparseImageFormatProperties[count];
            for (var i = 0; i < count; ++i)
            {
                pProperties[i] = TranslateSparseImageFormatProperties(ref formatProperties[i]);
            }
        }

        internal static MgSparseImageFormatProperties TranslateSparseImageFormatProperties(ref VkSparseImageFormatProperties src)
        {
            return new MgSparseImageFormatProperties
            {
                AspectMask = src.aspectMask,
                ImageGranularity = src.imageGranularity,
                Flags = (MgSparseImageFormatFlagBits)src.flags,
            };
        }
    }
}
