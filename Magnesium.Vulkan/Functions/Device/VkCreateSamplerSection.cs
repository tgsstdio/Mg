using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateSamplerSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateSampler(IntPtr device, [In, Out] VkSamplerCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSampler);

		public static MgResult CreateSampler(VkDeviceInfo info, MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
		{
			// TODO: add implementation
		}
	}
}
