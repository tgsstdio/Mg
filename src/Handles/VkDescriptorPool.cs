using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkDescriptorPool : IMgDescriptorPool
	{
		internal UInt64 Handle = 0L;
		internal VkDescriptorPool(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyDescriptorPool(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			IntPtr devicePtr = bDevice != null ? bDevice.Handle : IntPtr.Zero;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyDescriptorPool(devicePtr, this.Handle, allocatorPtr);
		}

		public Result ResetDescriptorPool(IMgDevice device, UInt32 flags)
		{
			var bDevice = device as VkDevice;
			IntPtr devicePtr = bDevice != null ? bDevice.Handle : IntPtr.Zero;

			return (Result)Interops.vkResetDescriptorPool(devicePtr, this.Handle, (UInt32) flags);
		}

	}
}
