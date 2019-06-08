using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateDescriptorPoolSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateDescriptorPool(IntPtr device, [In, Out] VkDescriptorPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pDescriptorPool);

		public static MgResult CreateDescriptorPool(VkDeviceInfo info, MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
		{
			// TODO: add implementation
		}
	}
}
