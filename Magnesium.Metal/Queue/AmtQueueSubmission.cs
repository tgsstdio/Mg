using System.Collections.Generic;

namespace Magnesium.Metal
{
	public class AmtQueueSubmission
	{
		public long Key;
		public AmtQueueSubmission (long key, MgSubmitInfo sub)
		{
			Key = key;
			var waits = new List<AmtSemaphore> ();
			if (sub.WaitSemaphores != null)
			{
				foreach (var signal in sub.WaitSemaphores)
				{
					var semaphore = signal.WaitSemaphore as AmtSemaphore;
					if (semaphore != null)
					{
						waits.Add (semaphore);
					}
				}
			}
			Waits = waits.ToArray ();

			var signals = new List<AmtSemaphore> ();
			if (sub.SignalSemaphores != null)
			{
				foreach (var signal in sub.SignalSemaphores)
				{
					var semaphore = signal as AmtSemaphore;
					if (semaphore != null)
					{
						signals.Add (semaphore);
					}
				}
			}
			Signals = signals.ToArray ();

			var buffers = new List<AmtCommandBuffer> ();
			if (sub.CommandBuffers != null)
			{
				foreach (var buf in sub.CommandBuffers)
				{
					var glCmdBuf = buf as AmtCommandBuffer;
					if (glCmdBuf != null)
					{
						buffers.Add (glCmdBuf);
					}
				}
			}

			CommandBuffers = buffers.ToArray();
		}
		public AmtSemaphore[] Waits { get; private set; }

		public AmtCommandBuffer[] CommandBuffers { get; private set; }

		public AmtSemaphore[] Signals { get; private set; }
		public IAmtFence OrderFence { get; set; }
		public AmtQueueSwapchainInfo[] Swapchains { get; set; }
	}
}

