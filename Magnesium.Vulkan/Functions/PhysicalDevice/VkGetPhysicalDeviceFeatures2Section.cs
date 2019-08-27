using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceFeatures2Section
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceFeatures2(IntPtr physicalDevice, ref VkPhysicalDeviceFeatures2 pFeatures);

        public static void GetPhysicalDeviceFeatures2(VkPhysicalDeviceInfo info, out MgPhysicalDeviceFeatures2 pFeatures)
        {
            var bFeatures = new VkPhysicalDeviceFeatures2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceFeatures2,
                pNext = IntPtr.Zero, // TODO: extension
            };

            vkGetPhysicalDeviceFeatures2(info.Handle, ref bFeatures);

            pFeatures = new MgPhysicalDeviceFeatures2
            {
                Features = VkGetPhysicalDeviceFeaturesSection.TranslateFeatures(ref bFeatures.features),
            };
        }
    }
}
