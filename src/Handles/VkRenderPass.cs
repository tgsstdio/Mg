using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkRenderPass : IMgRenderPass
	{
		internal UInt64 Handle = 0L;
		internal VkRenderPass(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyRenderPass(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			IntPtr devicePtr = bDevice != null ? bDevice.Handle : IntPtr.Zero;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyRenderPass(devicePtr, this.Handle, allocatorPtr);
		}

	}
}
