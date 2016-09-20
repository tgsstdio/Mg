using System;
namespace Magnesium.Metal
{
	public class AmtQueue : IMgQueue
	{
		public AmtQueue()
		{
		}

		public Result QueueBindSparse(MgBindSparseInfo[] pBindInfo, IMgFence fence)
		{
			throw new NotImplementedException();
		}

		public Result QueuePresentKHR(MgPresentInfoKHR pPresentInfo)
		{
			throw new NotImplementedException();
		}

		public Result QueueSubmit(MgSubmitInfo[] pSubmits, IMgFence fence)
		{
			throw new NotImplementedException();
		}

		public Result QueueWaitIdle()
		{
			throw new NotImplementedException();
		}
	}
}
