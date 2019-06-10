using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceFormatPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkGetPhysicalDeviceFormatProperties(IntPtr physicalDevice, MgFormat format, ref VkFormatProperties pFormatProperties);

        public static void GetPhysicalDeviceFormatProperties(VkPhysicalDeviceInfo info, MgFormat format, out MgFormatProperties pFormatProperties)
        {
            var formatProperties = default(VkFormatProperties);
            vkGetPhysicalDeviceFormatProperties(info.Handle, format, ref formatProperties);

            pFormatProperties = TranslateFormatProperties(format, ref formatProperties);
        }

        internal static MgFormatProperties TranslateFormatProperties(MgFormat format, ref VkFormatProperties formatProperties)
        {
            return new MgFormatProperties
            {
                Format = format,
                LinearTilingFeatures = (MgFormatFeatureFlagBits)formatProperties.linearTilingFeatures,
                OptimalTilingFeatures = (MgFormatFeatureFlagBits)formatProperties.optimalTilingFeatures,
                BufferFeatures = (MgFormatFeatureFlagBits)formatProperties.bufferFeatures,
            };
        }
    }
}
