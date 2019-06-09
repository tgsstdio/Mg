using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceMultisamplePropertiesEXTSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetPhysicalDeviceMultisamplePropertiesEXT(IntPtr physicalDevice, MgSampleCountFlagBits samples, VkMultisamplePropertiesEXT pMultisampleProperties);

		public static void GetPhysicalDeviceMultisamplePropertiesEXT(VkPhysicalDeviceInfo info, MgSampleCountFlagBits samples, MgMultisamplePropertiesEXT pMultisampleProperties)
		{
			// TODO: add implementation
		}
	}
}
