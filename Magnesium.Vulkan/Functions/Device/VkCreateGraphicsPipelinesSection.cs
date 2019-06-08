using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateGraphicsPipelinesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateGraphicsPipelines(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkGraphicsPipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, UInt64[] pPipelines);

		public static MgResult CreateGraphicsPipelines(VkDeviceInfo info, IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			// TODO: add implementation
		}
	}
}
