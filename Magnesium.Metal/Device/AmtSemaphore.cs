using System;
using System.Threading;

namespace Magnesium.Metal
{
	public class AmtSemaphore : IMgSemaphore
	{
		private volatile bool mIsSignalled;
		public bool IsSignalled
		{
			get
			{
				return mIsSignalled;
			}
		}

		public AmtSemaphore()
		{
			mIsSignalled = false;
		}

		internal void Reset()
		{
			mIsSignalled = false;
		}

		internal void Signal()
		{
			mIsSignalled = true;
		}

		public void DestroySemaphore(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
	}
}
