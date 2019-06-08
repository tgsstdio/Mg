using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateObjectTableNVXSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateObjectTableNVX(IntPtr device, [In, Out] VkObjectTableCreateInfoNVX pCreateInfo, IntPtr pAllocator, ref UInt64 pObjectTable);

		public static MgResult CreateObjectTableNVX(VkDeviceInfo info, MgObjectTableCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgObjectTableNVX pObjectTable)
		{
			// TODO: add implementation
		}
	}
}
