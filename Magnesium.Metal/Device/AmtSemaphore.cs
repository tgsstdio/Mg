using System;
using System.Threading;

namespace Magnesium.Metal
{
	public class AmtSemaphore : IMgSemaphore
	{
		private volatile bool mIsAlreadySignalled;
		public bool IsAlreadySignalled
		{
			get
			{
				return mIsAlreadySignalled;
			}
		}

		public AmtSemaphore()
		{
			mIsAlreadySignalled = true;
		}

		internal void Reset()
		{
			mIsAlreadySignalled = false;
		}

		internal void Signal()
		{
			mIsAlreadySignalled = true;
		}

		public void DestroySemaphore(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
	}
}
