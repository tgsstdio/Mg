using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceQueueFamilyPropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkGetPhysicalDeviceQueueFamilyProperties(IntPtr physicalDevice, UInt32* pQueueFamilyPropertyCount, VkQueueFamilyProperties* pQueueFamilyProperties);

        public static void GetPhysicalDeviceQueueFamilyProperties(VkPhysicalDeviceInfo info, out MgQueueFamilyProperties[] pQueueFamilyProperties)
        {
            unsafe
            {
                var count = stackalloc uint[1];
                count[0] = 0;
                Interops.vkGetPhysicalDeviceQueueFamilyProperties(info.Handle, count, null);

                var queueFamilyCount = (int)count[0];
                var familyProperties = stackalloc VkQueueFamilyProperties[queueFamilyCount];

                Interops.vkGetPhysicalDeviceQueueFamilyProperties(info.Handle, count, familyProperties);

                pQueueFamilyProperties = new MgQueueFamilyProperties[queueFamilyCount];
                for (var i = 0; i < queueFamilyCount; ++i)
                {
                    pQueueFamilyProperties[i] = TranslateQueueFamilyProperties(ref familyProperties[i]);
                }
            }
        }

        internal static unsafe MgQueueFamilyProperties TranslateQueueFamilyProperties(
            ref VkQueueFamilyProperties src)
        {
            return new MgQueueFamilyProperties
            {
                QueueFlags = src.queueFlags,
                QueueCount = src.queueCount,
                TimestampValidBits = src.timestampValidBits,
                MinImageTransferGranularity = src.minImageTransferGranularity,
            };
        }
    }
}
