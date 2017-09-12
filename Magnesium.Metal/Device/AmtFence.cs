using System.Threading;

namespace Magnesium.Metal
{
	public class AmtFence : IAmtFence
	{
		private int mSignals;
		public AmtFence()
		{
			mSignals = 0;
		}

		public bool AlreadySignalled
		{
			get
			{
				return mSignals <= 0;
			}
		}

		public void DestroyFence(IMgDevice device, IMgAllocationCallbacks allocator)
		{

		}

		public void Reset(int count)
		{
			mSignals = count;
		}

		public void Signal()
		{
			Interlocked.Decrement(ref mSignals);
		}
	}
}
