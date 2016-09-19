using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkPipelineLayout : IMgPipelineLayout
	{
		internal UInt64 Handle { get; private set; }
		internal VkPipelineLayout(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyPipelineLayout(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = (VkDevice)device;
			Debug.Assert(bDevice != null);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyPipelineLayout(bDevice.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}

	}
}
