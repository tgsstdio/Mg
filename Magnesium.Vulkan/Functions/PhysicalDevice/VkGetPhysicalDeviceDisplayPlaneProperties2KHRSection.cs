using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceDisplayPlaneProperties2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkGetPhysicalDeviceDisplayPlaneProperties2KHR(IntPtr physicalDevice, UInt32* pPropertyCount, VkDisplayPlaneProperties2KHR* pProperties);

		public static MgResult GetPhysicalDeviceDisplayPlaneProperties2KHR(VkPhysicalDeviceInfo info, out MgDisplayPlaneProperties2KHR[] pProperties)
		{
			// TODO: add implementation
		}
	}
}
