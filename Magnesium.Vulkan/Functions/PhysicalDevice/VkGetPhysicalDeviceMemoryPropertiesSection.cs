using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceMemoryPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceMemoryProperties(IntPtr physicalDevice, ref VkPhysicalDeviceMemoryProperties pMemoryProperties);

        public static void GetPhysicalDeviceMemoryProperties(VkPhysicalDeviceInfo info, out MgPhysicalDeviceMemoryProperties pMemoryProperties)
        {
            var memoryProperties = default(VkPhysicalDeviceMemoryProperties);
            Interops.vkGetPhysicalDeviceMemoryProperties(info.Handle, ref memoryProperties);

            pMemoryProperties = TranslateMemoryProperties(ref memoryProperties);
        }

        private static MgPhysicalDeviceMemoryProperties TranslateMemoryProperties(ref VkPhysicalDeviceMemoryProperties memoryProperties)
        {
            MgPhysicalDeviceMemoryProperties pMemoryProperties;
            var memoryHeaps = new MgMemoryHeap[memoryProperties.memoryHeapCount];
            for (var i = 0; i < memoryProperties.memoryHeapCount; ++i)
            {
                memoryHeaps[i] = new MgMemoryHeap
                {
                    Size = memoryProperties.memoryHeaps[i].size,
                    Flags = (MgMemoryHeapFlagBits)memoryProperties.memoryHeaps[i].flags,
                };
            }

            var memoryTypes = new MgMemoryType[memoryProperties.memoryTypeCount];
            for (var i = 0; i < memoryProperties.memoryTypeCount; ++i)
            {
                memoryTypes[i] = new MgMemoryType
                {
                    PropertyFlags = (uint)memoryProperties.memoryTypes[i].propertyFlags,
                    HeapIndex = memoryProperties.memoryTypes[i].heapIndex,
                };
            }

            pMemoryProperties = new MgPhysicalDeviceMemoryProperties
            {
                MemoryHeaps = memoryHeaps,
                MemoryTypes = memoryTypes,
            };
            return pMemoryProperties;
        }
    }
}
