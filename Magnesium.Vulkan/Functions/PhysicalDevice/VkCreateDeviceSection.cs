using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkCreateDeviceSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateDevice(IntPtr physicalDevice, [In, Out] VkDeviceCreateInfo pCreateInfo, IntPtr pAllocator, ref IntPtr pDevice);

		public static MgResult CreateDevice(VkPhysicalDeviceInfo info, MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice)
		{
			// TODO: add implementation
		}
	}
}
