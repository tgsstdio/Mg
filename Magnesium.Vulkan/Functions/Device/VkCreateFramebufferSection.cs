using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateFramebufferSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateFramebuffer(IntPtr device, [In, Out] VkFramebufferCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pFramebuffer);

		public static MgResult CreateFramebuffer(VkDeviceInfo info, MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
		{
			// TODO: add implementation
		}
	}
}
