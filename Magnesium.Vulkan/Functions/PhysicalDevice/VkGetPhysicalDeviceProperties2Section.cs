using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceProperties2(IntPtr physicalDevice, ref VkPhysicalDeviceProperties2 pProperties);

        public static void GetPhysicalDeviceProperties2(VkPhysicalDeviceInfo info, out MgPhysicalDeviceProperties2 pProperties)
        {
            var bProperties = new VkPhysicalDeviceProperties2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceProperties2,
                pNext = IntPtr.Zero, // TODO: extension
            };

            vkGetPhysicalDeviceProperties2(info.Handle, ref bProperties);

            pProperties = new MgPhysicalDeviceProperties2
            {
                Properties = VkGetPhysicalDevicePropertiesSection.TranslateDeviceProperties(ref bProperties.properties),
            };
        }
    }
}
