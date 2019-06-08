using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDevicePresentRectanglesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe VkResult vkGetPhysicalDevicePresentRectanglesKHR(IntPtr physicalDevice, UInt64 surface, UInt32* pRectCount, VkRect2D* pRects);

		public static MgResult GetPhysicalDevicePresentRectanglesKHR(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, MgRect2D[] pRects)
		{
			// TODO: add implementation
		}
	}
}
