using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkImage : IMgImage
	{
		internal UInt64 Handle { get; private set;}
		internal VkImage(UInt64 handle)
		{
			Handle = handle;
		}

		public Result BindImageMemory(IMgDevice device, IMgDeviceMemory memory, UInt64 memoryOffset)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);

			var bMemory = (VkDeviceMemory) memory;
			Debug.Assert(bMemory != null);

			return Interops.vkBindImageMemory(bDevice.Handle, this.Handle, bMemory.Handle, memoryOffset);
		}

		private bool mIsDisposed = false;
		public void DestroyImage(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);			
			
			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;	

			Interops.vkDestroyImage(bDevice.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
			
		}

	}
}
