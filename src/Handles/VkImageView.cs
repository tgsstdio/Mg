using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkImageView : IMgImageView
	{
		internal UInt64 Handle = 0L;
		internal VkImageView(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyImageView(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			IntPtr devicePtr = bDevice != null ? bDevice.Handle : IntPtr.Zero;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyImageView(devicePtr, this.Handle, allocatorPtr);			
		}

	}
}
