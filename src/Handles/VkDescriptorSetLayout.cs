using Magnesium;
using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkDescriptorSetLayout : IMgDescriptorSetLayout
	{
		internal UInt64 Handle { get; private set;}
		internal VkDescriptorSetLayout(UInt64 handle)
		{
			Handle = handle;
		}

		private bool mIsDisposed = false;
		public void DestroyDescriptorSetLayout(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;
			
			var bDevice = device as VkDevice;
			var bAllocator = allocator as MgVkAllocationCallbacks;

			Debug.Assert(bDevice != null);

			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyDescriptorSetLayout (bDevice.Handle, this.Handle, allocatorPtr);

			mIsDisposed = true;
		}

	}
}
