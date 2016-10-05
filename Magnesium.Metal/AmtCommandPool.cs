using System;
using System.Collections.Generic;
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

			mBuffers = new List<AmtCommandBuffer>();
		}

		private bool mIsDisposed = false;
		public void DestroyCommandPool(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mBuffers.Clear();

			mIsDisposed = true;
		}

		public Result ResetCommandPool(IMgDevice device, MgCommandPoolResetFlagBits flags)
		{
			foreach (var buffer in mBuffers)
			{
				buffer.ResetAllData();
			}
			return Result.SUCCESS;
		}

		private List<AmtCommandBuffer> mBuffers;
		internal void Add(AmtCommandBuffer cmdBuf)
		{
			mBuffers.Add(cmdBuf);
		}
	}
}