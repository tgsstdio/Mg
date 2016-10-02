using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtCommandPool : IMgCommandPool
	{
		private readonly IMTLCommandQueue mQueue;

		public IMTLCommandQueue Queue
		{
			get
			{
				return mQueue;
			}
		}

		public bool CanIndividuallyReset { get; private set; }

		public AmtCommandPool(IMTLCommandQueue queue, MgCommandPoolCreateInfo pCreateInfo)
		{
			mQueue = queue;

			CanIndividuallyReset = (pCreateInfo.Flags & MgCommandPoolCreateFlagBits.RESET_COMMAND_BUFFER_BIT)
				== MgCommandPoolCreateFlagBits.RESET_COMMAND_BUFFER_BIT;
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