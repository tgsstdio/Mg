using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkBuffer : IMgBuffer
	{
		internal UInt64 Handle = 0L;
		internal VkBuffer(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyBuffer(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			Debug.Assert(bDevice != null);
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyBuffer(bDevice.Handle, this.Handle, allocatorPtr);

			mIsDisposed = true;
		}

		public Result BindBufferMemory(IMgDevice device, IMgDeviceMemory memory, UInt64 memoryOffset)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = device as VkDevice;
			var bMemory = memory as VkDeviceMemory;

			Debug.Assert(bDevice != null); // RIGHT TYPE
			Debug.Assert(bMemory != null); // RIGHT TYPE

			return (Result)Interops.vkBindBufferMemory(bDevice.Handle, this.Handle, bMemory.Handle, memoryOffset); 
		}

	}
}
