using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceDisplayPlaneProperties2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceDisplayPlaneProperties2KHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayPlaneProperties2KHR[] pProperties);

        public static MgResult GetPhysicalDeviceDisplayPlaneProperties2KHR(VkPhysicalDeviceInfo info, out MgDisplayPlaneProperties2KHR[] pProperties)
        {
            uint count = 0;
            var first = vkGetPhysicalDeviceDisplayPlaneProperties2KHR(info.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var planeProperties = new VkDisplayPlaneProperties2KHR[count];

            for (var i = 0; i < count; i += 1)
            {
                planeProperties[i] = new VkDisplayPlaneProperties2KHR
                {
                    sType = VkStructureType.StructureTypeDisplayPlaneProperties2Khr,
                    // TODO: extension
                    pNext = IntPtr.Zero,
                };
            }

            var final = vkGetPhysicalDeviceDisplayPlaneProperties2KHR(info.Handle, ref count, planeProperties);

            pProperties = new MgDisplayPlaneProperties2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                var currentProperty = planeProperties[i].displayPlaneProperties;
                pProperties[i] = new MgDisplayPlaneProperties2KHR
                {
                    DisplayPlaneProperties = new MgDisplayPlanePropertiesKHR
                    {
                        CurrentDisplay = new VkDisplayKHR(currentProperty.currentDisplay),
                        CurrentStackIndex = currentProperty.currentStackIndex,
                    }
                };
            }

            return final;
        }
    }
}
