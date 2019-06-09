using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceSurfaceSupportKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfaceSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, UInt64 surface, ref VkBool32 pSupported);

		public static MgResult GetPhysicalDeviceSurfaceSupportKHR(VkPhysicalDeviceInfo info, UInt32 queueFamilyIndex, IMgSurfaceKHR surface, ref Boolean pSupported)
		{
			// TODO: add implementation
		}
	}
}
