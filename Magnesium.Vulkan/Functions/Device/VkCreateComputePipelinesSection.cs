using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateComputePipelinesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateComputePipelines(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkComputePipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, UInt64[] pPipelines);

		public static MgResult CreateComputePipelines(VkDeviceInfo info, IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			// TODO: add implementation
		}
	}
}
