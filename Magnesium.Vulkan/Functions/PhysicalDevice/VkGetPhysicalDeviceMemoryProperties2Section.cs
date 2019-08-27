using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceMemoryProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceMemoryProperties2(IntPtr physicalDevice, ref VkPhysicalDeviceMemoryProperties2 pMemoryProperties);

        public static void GetPhysicalDeviceMemoryProperties2(VkPhysicalDeviceInfo info, out MgPhysicalDeviceMemoryProperties2 pMemoryProperties)
        {
            var bMemoryProperties = new VkPhysicalDeviceMemoryProperties2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceMemoryProperties2,
                pNext = IntPtr.Zero, // TODO: extension
            };

            vkGetPhysicalDeviceMemoryProperties2(info.Handle, ref bMemoryProperties);

            pMemoryProperties = new MgPhysicalDeviceMemoryProperties2
            {
                MemoryProperties = VkGetPhysicalDeviceMemoryPropertiesSection.TranslateMemoryProperties(ref bMemoryProperties.memoryProperties),
            };
        }
    }
}
