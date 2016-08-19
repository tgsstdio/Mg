using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkPipeline : IMgPipeline
	{
		internal UInt64 Handle = 0L;
		internal VkPipeline(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyPipeline(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			IntPtr devicePtr = bDevice != null ? bDevice.Handle : IntPtr.Zero;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyPipeline(devicePtr, this.Handle, allocatorPtr);			
		}

	}
}
