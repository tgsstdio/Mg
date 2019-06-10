using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceDisplayProperties2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPhysicalDeviceDisplayProperties2KHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayProperties2KHR[] pProperties);

        public static MgResult GetPhysicalDeviceDisplayProperties2KHR(VkPhysicalDeviceInfo info, out MgDisplayProperties2KHR[] pProperties)
        {
            uint count = 0;
            var first = vkGetPhysicalDeviceDisplayProperties2KHR(info.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var displayProperties = new VkDisplayProperties2KHR[count];

            for (var i = 0; i < count; i += 1)
            {
                displayProperties[i] = new VkDisplayProperties2KHR
                {
                    sType = VkStructureType.StructureTypeDisplayProperties2Khr,
                    // TODO: extension
                    pNext = IntPtr.Zero,
                };
            }

            var final = vkGetPhysicalDeviceDisplayProperties2KHR(info.Handle, ref count, displayProperties);

            pProperties = new MgDisplayProperties2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                var current = displayProperties[i].displayProperties;
                var internalDisplay = new VkDisplayKHR(current.display);

                pProperties[i] = new MgDisplayProperties2KHR
                {
                    DisplayProperties = new MgDisplayPropertiesKHR
                    {
                        Display = internalDisplay,
                        DisplayName = current.displayName,
                        PhysicalDimensions = current.physicalDimensions,
                        PhysicalResolution = current.physicalResolution,
                        SupportedTransforms = (MgSurfaceTransformFlagBitsKHR)current.supportedTransforms,
                        PlaneReorderPossible = VkBool32.ConvertFrom(current.planeReorderPossible),
                        PersistentContent = VkBool32.ConvertFrom(current.persistentContent),
                    }
                };
            }
            return final;
        }
    }
}
