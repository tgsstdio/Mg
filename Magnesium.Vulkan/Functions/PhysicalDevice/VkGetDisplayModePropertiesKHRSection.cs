using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetDisplayModePropertiesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkGetDisplayModePropertiesKHR(IntPtr physicalDevice, UInt64 display, UInt32* pPropertyCount, VkDisplayModePropertiesKHR* pProperties);

		public static MgResult GetDisplayModePropertiesKHR(VkPhysicalDeviceInfo info, IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
		{
			// TODO: add implementation
		}
	}
}
