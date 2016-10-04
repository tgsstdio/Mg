using System;
using System.Threading;

namespace Magnesium.Metal
{
	public class AmtSemaphore
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
	}
}
