using System;
using Magnesium;

namespace Magnesium
{
	public class MgThreadPartition : IMgThreadPartition
	{
		private readonly IMgPhysicalDevice mPhysicalDevice;
		private readonly IMgDevice mDevice;
		public MgThreadPartition (
			IMgPhysicalDevice physicalDevice,
			IMgDevice device, IMgQueue queue, 
			IMgCommandPool commandPool,
			MgPhysicalDeviceMemoryProperties deviceMemoryProperties)
		{
			mPhysicalDevice = physicalDevice;
			mDevice = device;
			this.Queue = queue;
			this.CommandPool = commandPool;
			mDeviceMemoryProperties = deviceMemoryProperties;
		}

		#region IDisposable implementation

		~MgThreadPartition()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private bool mIsDisposed = false;
		protected void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			ReleaseUnmanagedResources ();

			if (disposing)
				ReleaseManagedResources ();			

			mIsDisposed = true;
		}

		protected virtual void ReleaseUnmanagedResources ()
		{
			// Command buffers first
			CommandPool.DestroyCommandPool (Device, null);
		}

		protected virtual void ReleaseManagedResources ()
		{
			
		}

		#endregion

		#region IMgThreadPartition implementation

		private MgPhysicalDeviceMemoryProperties mDeviceMemoryProperties;
		public bool GetMemoryType(uint typeBits, MgMemoryPropertyFlagBits memoryPropertyFlags, out uint typeIndex)
		{
			uint requirements = (uint)memoryPropertyFlags;

			// Search memtypes to find first index with those properties
			for (UInt32 i = 0; i < mDeviceMemoryProperties.MemoryTypes.Length; i++)
			{
				if ((typeBits & 1) == 1)
				{
					// Type is available, does it match user properties?
					if ((mDeviceMemoryProperties.MemoryTypes[i].PropertyFlags & requirements) == requirements)
					{
						typeIndex = i;
						return true;
					}
				}
				typeBits >>= 1;
			}
			// No memory types matched, return failure
			typeIndex = 0;
			return false;
		}

		public IMgPhysicalDevice PhysicalDevice {
			get {
				return mPhysicalDevice;
			}	
		}

		public IMgCommandPool CommandPool {
			get;
			private set;
		}

		public IMgCommandBuffer[] CommandBuffers {
			get;
			private set;
		}

		public IMgQueue Queue {
			get;
			private set;
		}

		public IMgDevice Device {
			get {
				return mDevice;
			}
		}

		#endregion
	}
}

