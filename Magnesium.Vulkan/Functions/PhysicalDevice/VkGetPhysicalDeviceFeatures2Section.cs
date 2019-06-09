using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceFeatures2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetPhysicalDeviceFeatures2(IntPtr physicalDevice, VkPhysicalDeviceFeatures2* pFeatures);

		public static void GetPhysicalDeviceFeatures2(VkPhysicalDeviceInfo info, out MgPhysicalDeviceFeatures2 pFeatures)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
