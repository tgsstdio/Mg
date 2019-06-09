using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfacePresentModesKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfacePresentModesKHR(IntPtr physicalDevice, UInt64 surface, ref UInt32 pPresentModeCount, VkPresentModeKHR[] pPresentModes);

		public static MgResult GetPhysicalDeviceSurfacePresentModesKHR(VkPhysicalDeviceInfo info, IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
		{
			// TODO: add implementation
			throw new NotImplementedException();
		}
	}
}
