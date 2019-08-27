using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetDisplayModePropertiesKHRSection
	{
        [DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetDisplayModePropertiesKHR(IntPtr physicalDevice, UInt64 display, ref UInt32 pPropertyCount, [In, Out] VkDisplayModePropertiesKHR[] pProperties);

        public static MgResult GetDisplayModePropertiesKHR(VkPhysicalDeviceInfo info, IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
        {
            var bDisplay = (VkDisplayKHR)display;
            Debug.Assert(bDisplay != null); // MAYBE DUPLICATE CHECK
            uint count = 0;
            var first = vkGetDisplayModePropertiesKHR(info.Handle, bDisplay.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var modeProperties = new VkDisplayModePropertiesKHR[count];
            var final = vkGetDisplayModePropertiesKHR(info.Handle, bDisplay.Handle, ref count, modeProperties);

            pProperties = new MgDisplayModePropertiesKHR[count];
            for (var i = 0; i < count; ++i)
            {
                pProperties[i] = new MgDisplayModePropertiesKHR
                {
                    DisplayMode = new VkDisplayModeKHR(modeProperties[i].displayMode),
                    Parameters = modeProperties[i].parameters,
                };
            }

            return final;
        }
    }
}
