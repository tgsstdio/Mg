using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkFence : IMgFence
	{
		internal UInt64 Handle = 0L;
		internal VkFence(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyFence(IMgDevice device, IMgAllocationCallbacks allocator)
		{
		}

	}
}
