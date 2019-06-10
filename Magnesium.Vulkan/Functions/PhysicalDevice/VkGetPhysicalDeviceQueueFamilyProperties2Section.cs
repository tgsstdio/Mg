using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceQueueFamilyProperties2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceQueueFamilyProperties2(IntPtr physicalDevice, ref UInt32 pQueueFamilyPropertyCount, [In, Out] VkQueueFamilyProperties2[] pQueueFamilyProperties);

        public static void GetPhysicalDeviceQueueFamilyProperties2(VkPhysicalDeviceInfo info, out MgQueueFamilyProperties2[] pQueueFamilyProperties)
        {
            var count = 0U;
            vkGetPhysicalDeviceQueueFamilyProperties2(info.Handle, ref count, null);

            var bProperties = new VkQueueFamilyProperties2[count];

            for (var i = 0; i < count; i += 1)
            {
                bProperties[i] = new VkQueueFamilyProperties2
                {
                    sType = VkStructureType.StructureTypeQueueFamilyProperties2,
                    pNext = IntPtr.Zero, // TODO : extension
                };
            }

            if (count > 0)
            {
                vkGetPhysicalDeviceQueueFamilyProperties2(info.Handle, ref count, bProperties);
            }

            pQueueFamilyProperties = new MgQueueFamilyProperties2[count];

            for (var i = 0; i < count; i += 1)
            {
                pQueueFamilyProperties[i] = new MgQueueFamilyProperties2
                {
                    QueueFamilyProperties = VkGetPhysicalDeviceQueueFamilyPropertiesSection.TranslateQueueFamilyProperties(ref bProperties[i].queueFamilyProperties),
                };
            }
        }
    }
}
