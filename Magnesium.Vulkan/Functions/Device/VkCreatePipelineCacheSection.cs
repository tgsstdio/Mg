using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreatePipelineCacheSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreatePipelineCache(IntPtr device, [In, Out] VkPipelineCacheCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pPipelineCache);

		public static MgResult CreatePipelineCache(VkDeviceInfo info, MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
		{
			// TODO: add implementation
		}
	}
}
