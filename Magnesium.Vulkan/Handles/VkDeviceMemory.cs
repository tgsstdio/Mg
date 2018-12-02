using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkDeviceMemory : IMgDeviceMemory
	{
		internal UInt64 Handle { get; private set; }
		internal VkDeviceMemory(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void FreeMemory(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);			
			
			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;	

			Interops.vkFreeMemory(bDevice.Handle, this.Handle, allocatorPtr);

			this.Handle = 0UL;
			mIsDisposed = true;
		}

		public MgResult MapMemory(IMgDevice device, UInt64 offset, UInt64 size, UInt32 flags, out IntPtr ppData)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);

			IntPtr dest = IntPtr.Zero;
			var result = Interops.vkMapMemory(bDevice.Handle, this.Handle, offset, size, flags, ref dest);
			ppData = dest;
			return result;
		}

		public void UnmapMemory(IMgDevice device)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = (VkDevice) device;
			Debug.Assert(bDevice != null);		

			Interops.vkUnmapMemory(bDevice.Handle, this.Handle);
		}
	}
}
