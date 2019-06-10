using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceImageFormatProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceImageFormatProperties2(IntPtr physicalDevice, ref VkPhysicalDeviceImageFormatInfo2 pImageFormatInfo, ref VkImageFormatProperties2 pImageFormatProperties);

        public static MgResult GetPhysicalDeviceImageFormatProperties2(VkPhysicalDeviceInfo info, MgPhysicalDeviceImageFormatInfo2 pImageFormatInfo, MgImageFormatProperties2 pImageFormatProperties)
        {
            var bImageFormatInfo = new VkPhysicalDeviceImageFormatInfo2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceImageFormatInfo2,
                pNext = IntPtr.Zero, // TODO: extension
                type = (VkImageType)pImageFormatInfo.Type,
                flags = pImageFormatInfo.Flags,
                format = pImageFormatInfo.Format,
                tiling = pImageFormatInfo.Tiling,
                usage = pImageFormatInfo.Usage,
            };

            var output = new VkImageFormatProperties2
            {
                sType = VkStructureType.StructureTypeImageFormatProperties2,
                // TODO: extension
                pNext = IntPtr.Zero,
            };

            var result = vkGetPhysicalDeviceImageFormatProperties2(info.Handle, ref bImageFormatInfo, ref output);

            pImageFormatProperties = new MgImageFormatProperties2
            {
                ImageFormatProperties = new MgImageFormatProperties
                {
                    MaxExtent = output.imageFormatProperties.maxExtent,
                    MaxMipLevels = output.imageFormatProperties.maxMipLevels,
                    MaxArrayLayers = output.imageFormatProperties.maxArrayLayers,
                    SampleCounts = (MgSampleCountFlagBits)output.imageFormatProperties.sampleCounts,
                    MaxResourceSize = output.imageFormatProperties.maxResourceSize,
                }
            };
            return result;
        }
    }
}
