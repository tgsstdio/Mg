using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkImage : IMgImage
	{
		internal UInt64 Handle = 0L;
		internal VkImage(UInt64 handle)
		{
			Handle = handle;
		}

		public Result BindImageMemory(IMgDevice device, IMgDeviceMemory memory, UInt64 memoryOffset)
		{
			Debug.Assert(!mIsDisposed);

			var bDevice = device as VkDevice;
			var bMemory = memory as VkDeviceMemory;

			Debug.Assert(bDevice != null);
			Debug.Assert(bMemory != null);

			return (Magnesium.Result)Interops.vkBindImageMemory(bDevice.Handle, this.Handle, bMemory.Handle, memoryOffset);
		}

		private bool mIsDisposed = false;
		public void DestroyImage(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mIsDisposed = true;
			
		}

	}
}
