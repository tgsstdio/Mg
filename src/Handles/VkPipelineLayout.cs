using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkPipelineLayout : IMgPipelineLayout
	{
		internal UInt64 Handle = 0L;
		internal VkPipelineLayout(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyPipelineLayout(IMgDevice device, IMgAllocationCallbacks allocator)
		{
		}

	}
}
