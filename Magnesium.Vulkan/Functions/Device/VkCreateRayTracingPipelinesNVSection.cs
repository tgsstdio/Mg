using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateRayTracingPipelinesNVSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateRayTracingPipelinesNV(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkRayTracingPipelineCreateInfoNV[] pCreateInfos, IntPtr pAllocator, UInt64[] pPipelines);

		public static MgResult CreateRayTracingPipelinesNV(VkDeviceInfo info, IMgPipelineCache pipelineCache, MgRayTracingPipelineCreateInfoNV[] pCreateInfos, IMgAllocationCallbacks pAllocator, out IMgPipeline[] pPipelines)
		{
			// TODO: add implementation
		}
	}
}
