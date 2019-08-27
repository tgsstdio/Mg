using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceDisplayPlanePropertiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceDisplayPlanePropertiesKHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayPlanePropertiesKHR[] pProperties);

        public static MgResult GetPhysicalDeviceDisplayPlanePropertiesKHR(VkPhysicalDeviceInfo info, out MgDisplayPlanePropertiesKHR[] pProperties)
        {
            uint count = 0;
            var first = vkGetPhysicalDeviceDisplayPlanePropertiesKHR(info.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var planeProperties = new VkDisplayPlanePropertiesKHR[count];
            var final = vkGetPhysicalDeviceDisplayPlanePropertiesKHR(info.Handle, ref count, planeProperties);

            pProperties = new MgDisplayPlanePropertiesKHR[count];
            for (var i = 0; i < count; ++i)
            {
                pProperties[i] = new MgDisplayPlanePropertiesKHR
                {
                    CurrentDisplay = new VkDisplayKHR(planeProperties[i].currentDisplay),
                    CurrentStackIndex = planeProperties[i].currentStackIndex,
                };
            }

            return final;
        }
    }
}
