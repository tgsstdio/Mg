using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkCommandPool : IMgCommandPool
	{
		internal UInt64 Handle { get; private set; }
		internal VkCommandPool(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyCommandPool(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);

			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyCommandPool(bDevice.mHandle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}

		public MgResult ResetCommandPool(IMgDevice device, MgCommandPoolResetFlagBits flags)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);

			return Interops.vkResetCommandPool(bDevice.mHandle, this.Handle, (VkCommandPoolResetFlags)flags);
		}

	}
}
