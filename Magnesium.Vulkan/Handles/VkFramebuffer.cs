using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkFramebuffer : IMgFramebuffer
	{
		internal UInt64 Handle { get; private set;}
		internal VkFramebuffer(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyFramebuffer(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			Debug.Assert(bDevice != null);

			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyFramebuffer(bDevice.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}

	}
}
