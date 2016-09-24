using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtCommandPool : IMgCommandPool
	{
		private IMTLCommandQueue mQueue;
		public IMTLCommandQueue Queue {get; private set;}
		public AmtCommandPool(IMTLCommandQueue queue)
		{
			mQueue = queue;
		}

		public void DestroyCommandPool(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}

		public Result ResetCommandPool(IMgDevice device, MgCommandPoolResetFlagBits flags)
		{
			throw new NotImplementedException();
		}
	}
}