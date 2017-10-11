using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkDebugReportCallbackEXT : IMgDebugReportCallbackEXT
	{
		internal UInt64 Handle { get; private set; }
        PFN_MgDebugReportCallbackEXT Callback { get; set; }
		public VkDebugReportCallbackEXT(UInt64 handle, PFN_MgDebugReportCallbackEXT callback)
		{
			this.Handle = handle;
            this.Callback = callback;
		}

		private bool mIsDisposed = false;
		public void DestroyDebugReportCallbackEXT(IMgInstance instance, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bInstance = (VkInstance)instance;
			Debug.Assert(bInstance != null);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyDebugReportCallbackEXT(bInstance.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
            this.Callback = null;
			mIsDisposed = true;
		}
	}
}