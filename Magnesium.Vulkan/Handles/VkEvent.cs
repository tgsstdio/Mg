using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkEvent : IMgEvent
	{
		internal UInt64 Handle { get; private set;}
		internal VkEvent(UInt64 handle)
		{
			Handle = handle;
		}

		public MgResult GetEventStatus(IMgDevice device)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);		

			return Interops.vkGetEventStatus(bDevice.Handle, this.Handle);
		}

		public MgResult SetEvent(IMgDevice device)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);		

			return Interops.vkSetEvent(bDevice.Handle, this.Handle);
		}

		public MgResult ResetEvent(IMgDevice device)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);		

			return (MgResult)Interops.vkResetEvent(bDevice.Handle, this.Handle);
		}

		private bool mIsDisposed = false;
		public void DestroyEvent(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);			
			
			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;	

			Interops.vkDestroyEvent(bDevice.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}

	}
}
