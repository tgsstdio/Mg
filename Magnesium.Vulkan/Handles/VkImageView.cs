using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkImageView : IMgImageView
	{
		internal UInt64 Handle { get; private set; }
		internal VkImageView(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyImageView(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;	

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);			
			
			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;	

			Interops.vkDestroyImageView(bDevice.Info.Handle, this.Handle, allocatorPtr);	

			this.Handle = 0UL;
			mIsDisposed = true;
		}

	}
}
