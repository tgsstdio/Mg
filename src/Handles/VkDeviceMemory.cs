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

			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			Debug.Assert(bDevice != null);

			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkFreeMemory(bDevice.Handle, this.Handle, allocatorPtr);

			mIsDisposed = true;
		}

		public Result MapMemory(IMgDevice device, UInt64 offset, UInt64 size, UInt32 flags, out IntPtr ppData)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = device as VkDevice;
			Debug.Assert(bDevice != null);

			IntPtr dest = IntPtr.Zero;
			var result = Interops.vkMapMemory(bDevice.Handle, this.Handle, offset, size, (UInt32)flags, ref dest);
			ppData = dest;
			return result;
		}

		public void UnmapMemory(IMgDevice device)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = device as VkDevice;
			Debug.Assert(bDevice != null);

			Interops.vkUnmapMemory(bDevice.Handle, this.Handle);
		}
	}
}
