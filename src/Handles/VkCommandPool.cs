using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkCommandPool : IMgCommandPool
	{
		internal UInt64 Handle = 0L;
		internal VkCommandPool(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyCommandPool(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			IntPtr devicePtr = bDevice != null ? bDevice.Handle : IntPtr.Zero;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyCommandPool(devicePtr, this.Handle, allocatorPtr);			
		}

		public Result ResetCommandPool(IMgDevice device, MgCommandPoolResetFlagBits flags)
		{
			var bDevice = device as VkDevice;

			IntPtr devicePtr = bDevice != null ? bDevice.Handle : IntPtr.Zero;
			return (Result) Interops.vkResetCommandPool(devicePtr, this.Handle, (VkCommandPoolResetFlags)flags);
		}

	}
}
