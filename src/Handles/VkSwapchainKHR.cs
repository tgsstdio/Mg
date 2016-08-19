using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkSwapchainKHR : IMgSwapchainKHR
	{
		public UInt64 Handle { get; private set; }
		public VkSwapchainKHR(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroySwapchainKHR(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			Debug.Assert(bDevice != null);

			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroySwapchainKHR(bDevice.Handle, this.Handle, allocatorPtr);

			mIsDisposed = true;
		}

	}
}
