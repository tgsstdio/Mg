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
			// TODO: add implementation
			throw new NotImplementedException();
		}
	}
}
