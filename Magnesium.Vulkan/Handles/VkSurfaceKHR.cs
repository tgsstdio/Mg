using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkSurfaceKHR : IMgSurfaceKHR
	{
		internal UInt64 Handle { get; private set; }
		public VkSurfaceKHR(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroySurfaceKHR(IMgInstance instance, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bInstance = (VkInstance)instance;
			Debug.Assert(bInstance != null);

			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroySurfaceKHR(bInstance.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}
	}
}
