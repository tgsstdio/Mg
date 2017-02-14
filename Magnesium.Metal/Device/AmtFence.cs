using System;
namespace Magnesium.Metal
{
	public class AmtFence : IAmtFence
	{
		private bool mAlreadySignalled;
		public AmtFence()
		{
			mAlreadySignalled = true;
		}

		public bool AlreadySignalled
		{
			get
			{
				return mAlreadySignalled;
			}
		}

		public void DestroyFence(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}

		public void Reset()
		{
			mAlreadySignalled = false;
		}

		public void Signal()
		{
			mAlreadySignalled = true;
		}
	}
}
