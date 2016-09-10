using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	class GLQueueSubmission
	{
		public uint Key;
		public GLQueueSubmission (uint key, MgSubmitInfo sub)
		{
			Key = key;
			var waits = new List<IGLSemaphore> ();
			if (sub.WaitSemaphores != null)
			{
				foreach (var signal in sub.WaitSemaphores)
				{
					var semaphore = signal.WaitSemaphore as IGLSemaphore;
					if (semaphore != null)
					{
						waits.Add (semaphore);
					}
				}
			}
			Waits = waits.ToArray ();

			var signals = new List<IGLSemaphore> ();
			if (sub.SignalSemaphores != null)
			{
				foreach (var signal in sub.SignalSemaphores)
				{
					var semaphore = signal as IGLSemaphore;
					if (semaphore != null)
					{
						signals.Add (semaphore);
					}
				}
			}
			Signals = signals.ToArray ();

			var buffers = new List<GLCommandBuffer> ();
			if (sub.CommandBuffers != null)
			{
				foreach (var buf in sub.CommandBuffers)
				{
					var glCmdBuf = buf as GLCommandBuffer;
					if (glCmdBuf != null)
					{
						buffers.Add (glCmdBuf);
					}
				}
			}

			CommandBuffers = buffers.ToArray();
		}
		public IGLSemaphore[] Waits { get; private set; }

		public GLCommandBuffer[] CommandBuffers { get; private set; }

		public IGLSemaphore[] Signals { get; private set; }
		public IGLSemaphore OrderFence { get; set; }
	}
}

