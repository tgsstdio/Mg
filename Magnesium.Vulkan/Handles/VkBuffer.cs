using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkBuffer : IMgBuffer
	{
		internal UInt64 Handle { get; private set; }
		internal VkBuffer(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyBuffer(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);

			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyBuffer(bDevice.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}

		public MgResult BindBufferMemory(IMgDevice device, IMgDeviceMemory memory, UInt64 memoryOffset)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null); // RIGHT TYPE

			var bMemory = (VkDeviceMemory) memory;
			Debug.Assert(bMemory != null); // RIGHT TYPE

			return Interops.vkBindBufferMemory(bDevice.Handle, this.Handle, bMemory.Handle, memoryOffset); 
		}

	}
}
