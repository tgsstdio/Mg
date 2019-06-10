using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceWin32PresentationSupportKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static VkBool32 vkGetPhysicalDeviceWin32PresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex);

        public static Boolean GetPhysicalDeviceWin32PresentationSupportKHR(VkPhysicalDeviceInfo info, UInt32 queueFamilyIndex)
        {
            var final = vkGetPhysicalDeviceWin32PresentationSupportKHR(info.Handle, queueFamilyIndex);
            return VkBool32.ConvertFrom(final);
        }
    }
}
