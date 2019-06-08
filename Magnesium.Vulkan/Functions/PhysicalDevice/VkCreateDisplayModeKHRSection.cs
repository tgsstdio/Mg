using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkCreateDisplayModeKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateDisplayModeKHR(IntPtr physicalDevice, UInt64 display, [In, Out] VkDisplayModeCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pMode);

		public static MgResult CreateDisplayModeKHR(VkPhysicalDeviceInfo info, IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
		{
			// TODO: add implementation
		}
	}
}
