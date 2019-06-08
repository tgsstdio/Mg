using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateImageViewSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateImageView(IntPtr device, [In, Out] VkImageViewCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pView);

		public static MgResult CreateImageView(VkDeviceInfo info, MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView)
		{
			// TODO: add implementation
		}
	}
}
