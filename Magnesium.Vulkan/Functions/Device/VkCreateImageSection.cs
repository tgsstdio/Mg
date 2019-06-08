using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateImageSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateImage(IntPtr device, [In, Out] VkImageCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pImage);

		public static MgResult CreateImage(VkDeviceInfo info, MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage)
		{
			// TODO: add implementation
		}
	}
}
