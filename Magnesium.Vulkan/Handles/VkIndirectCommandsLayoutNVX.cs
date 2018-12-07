using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkIndirectCommandsLayoutNVX : IMgIndirectCommandsLayoutNVX
	{
		internal UInt64 Handle = 0L;
		internal VkIndirectCommandsLayoutNVX(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyIndirectCommandsLayoutNVX(IMgDevice device, IMgAllocationCallbacks allocator)
		{
		}

	}
}
