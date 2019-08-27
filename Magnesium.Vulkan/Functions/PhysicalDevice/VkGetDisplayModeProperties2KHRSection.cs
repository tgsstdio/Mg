using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetDisplayModeProperties2KHRSection
	{
        [DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetDisplayModeProperties2KHR(IntPtr physicalDevice, UInt64 display, ref UInt32 pPropertyCount, [In, Out] VkDisplayModeProperties2KHR[] pProperties);

        public static MgResult GetDisplayModeProperties2KHR(VkPhysicalDeviceInfo info, IMgDisplayKHR display, out MgDisplayModeProperties2KHR[] pProperties)
        {
            var bDisplay = (VkDisplayKHR)display;
            Debug.Assert(bDisplay != null); // MAYBE DUPLICATE CHECK
            uint count = 0;
            var first = vkGetDisplayModeProperties2KHR(info.Handle, bDisplay.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var modeProperties = new VkDisplayModeProperties2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                modeProperties[i] = new VkDisplayModeProperties2KHR
                {
                    sType = VkStructureType.StructureTypeDisplayModeProperties2Khr,
                    // TODO: extension modeProperties[i].pNext ???
                    pNext = IntPtr.Zero,
                };
            }

            var final = vkGetDisplayModeProperties2KHR(
                info.Handle, bDisplay.Handle, ref count, modeProperties);

            pProperties = new MgDisplayModeProperties2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                var current = modeProperties[i].displayModeProperties;
                pProperties[i] = new MgDisplayModeProperties2KHR
                {
                    DisplayModeProperties = new MgDisplayModePropertiesKHR
                    {
                        DisplayMode = new VkDisplayModeKHR(current.displayMode),
                        Parameters = current.parameters,
                    },
                };
            }

            return final;
        }
    }
}
