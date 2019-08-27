using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceFormatProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceFormatProperties2(IntPtr physicalDevice, MgFormat format, ref VkFormatProperties2 pFormatProperties);

        public static void GetPhysicalDeviceFormatProperties2(VkPhysicalDeviceInfo info, MgFormat format, out MgFormatProperties2 pFormatProperties)
        {
            var output = new VkFormatProperties2
            {
                sType = VkStructureType.StructureTypeFormatProperties2,
                pNext = IntPtr.Zero, // TODO: extension
            };
            vkGetPhysicalDeviceFormatProperties2(info.Handle, format, ref output);

            pFormatProperties = new MgFormatProperties2
            {
                FormatProperties = VkGetPhysicalDeviceFormatPropertiesSection.TranslateFormatProperties(format, ref output.formatProperties),
            };
        }
    }
}
