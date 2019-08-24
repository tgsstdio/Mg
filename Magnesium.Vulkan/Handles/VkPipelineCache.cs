using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkPipelineCache : IMgPipelineCache
	{
		internal UInt64 Handle { get; private set; }
		internal VkPipelineCache(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyPipelineCache(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = (VkDevice)device;
			Debug.Assert(bDevice != null);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyPipelineCache(bDevice.Info.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}

	}
}
