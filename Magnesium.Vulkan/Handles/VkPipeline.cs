using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkPipeline : IMgPipeline
	{
		internal UInt64 Handle { get; private set; }
		internal VkPipeline(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyPipeline(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = (VkDevice)device;
			Debug.Assert(bDevice != null);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyPipeline(bDevice.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}

	}
}
