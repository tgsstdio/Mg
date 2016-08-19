using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkPipelineCache : IMgPipelineCache
	{
		internal UInt64 Handle = 0L;
		internal VkPipelineCache(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyPipelineCache(IMgDevice device, IMgAllocationCallbacks allocator)
		{
		}

	}
}
