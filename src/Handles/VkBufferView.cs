using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkBufferView : IMgBufferView
	{
		internal UInt64 Handle = 0L;
		internal VkBufferView(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyBufferView(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			IntPtr devicePtr = bDevice != null ? bDevice.Handle : IntPtr.Zero;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyBufferView(devicePtr, this.Handle, allocatorPtr);
		}

	}
}
