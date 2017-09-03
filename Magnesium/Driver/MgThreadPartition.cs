using System;

namespace Magnesium
{
    public class MgThreadPartition : IMgThreadPartition
	{
		private readonly IMgPhysicalDevice mPhysicalDevice;
		private readonly IMgDevice mDevice;
		public MgThreadPartition (
			IMgPhysicalDevice physicalDevice,
			IMgDevice device, IMgQueue queue, 
			IMgCommandPool commandPool)
		{
			mPhysicalDevice = physicalDevice;
			mDevice = device;
			this.Queue = queue;
			this.CommandPool = commandPool;
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

		public IMgPhysicalDevice PhysicalDevice {
			get {
				return mPhysicalDevice;
			}	
		}

		public IMgCommandPool CommandPool {
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

