using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetDisplayModeProperties2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkGetDisplayModeProperties2KHR(IntPtr physicalDevice, UInt64 display, UInt32* pPropertyCount, VkDisplayModeProperties2KHR* pProperties);

		public static MgResult GetDisplayModeProperties2KHR(VkPhysicalDeviceInfo info, IMgDisplayKHR display, out MgDisplayModeProperties2KHR[] pProperties)
		{
			// TODO: add implementation
		}
	}
}
