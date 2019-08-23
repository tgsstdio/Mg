using System;

namespace Magnesium.Toolkit
{
	public class MgLogicalDevice : IMgLogicalDevice
	{
		public IMgPhysicalDevice GPU { get; private set; }
		public IMgDevice Device { get; private set; }
		public IMgQueueInfo[] Queues { get; private set; }

		public MgLogicalDevice (IMgPhysicalDevice gpu, IMgDevice device, IMgQueueInfo[] queues)
		{
			this.GPU = gpu;
			this.Device = device;
			this.Queues = queues;
		}

		#region IDisposable implementation

		~MgLogicalDevice()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private bool mIsDisposed = false;
		protected virtual void Dispose (bool disposed)
		{
			if (mIsDisposed)
				return;

			ReleaseUnmanagedResources ();

			if (disposed)
			{
				ReleaseManagedResources ();
			}

			mIsDisposed = true;
		}

		protected virtual void ReleaseManagedResources()
		{

		}	

		protected virtual void ReleaseUnmanagedResources ()
		{
			this.Device.DestroyDevice (null);
		}
		#endregion
	}
}

