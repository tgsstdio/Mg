using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkDescriptorPool : IMgDescriptorPool
	{
		internal UInt64 Handle { get; private set;}
		internal VkDescriptorPool(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyDescriptorPool(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);
			
			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyDescriptorPool(bDevice.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}

		public Result ResetDescriptorPool(IMgDevice device, UInt32 flags)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = device as VkDevice;
			Debug.Assert(bDevice != null);

			return Interops.vkResetDescriptorPool(bDevice.Handle, this.Handle, flags);
		}

	}
}
