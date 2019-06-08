using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateSemaphoreSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateSemaphore(IntPtr device, [In, Out] VkSemaphoreCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSemaphore);

		public static MgResult CreateSemaphore(VkDeviceInfo info, MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
		{
			// TODO: add implementation
		}
	}
}
