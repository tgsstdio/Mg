using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceDisplayPropertiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceDisplayPropertiesKHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayPropertiesKHR[] pProperties);

        public static MgResult GetPhysicalDeviceDisplayPropertiesKHR(VkPhysicalDeviceInfo info, out MgDisplayPropertiesKHR[] pProperties)
        {
            uint count = 0;
            var first = vkGetPhysicalDeviceDisplayPropertiesKHR(info.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var displayProperties = new VkDisplayPropertiesKHR[count];
            var final = vkGetPhysicalDeviceDisplayPropertiesKHR(info.Handle, ref count, displayProperties);

            pProperties = new MgDisplayPropertiesKHR[count];
            for (var i = 0; i < count; ++i)
            {
                var internalDisplay = new VkDisplayKHR(displayProperties[i].display);

                pProperties[i] = new MgDisplayPropertiesKHR
                {
                    Display = internalDisplay,
                    DisplayName = displayProperties[i].displayName,
                    PhysicalDimensions = displayProperties[i].physicalDimensions,
                    PhysicalResolution = displayProperties[i].physicalResolution,
                    SupportedTransforms = (MgSurfaceTransformFlagBitsKHR)displayProperties[i].supportedTransforms,
                    PlaneReorderPossible = VkBool32.ConvertFrom(displayProperties[i].planeReorderPossible),
                    PersistentContent = VkBool32.ConvertFrom(displayProperties[i].persistentContent),
                };
            }
            return final;
        }
    }
}
