using System.Collections.Generic;
using Metal;

namespace Magnesium.Metal
{
	public class AmtQueueRenderer : IAmtQueueRenderer
	{
		public IMTLCommandBuffer[] Render(GLQueueSubmission request)
		{
			var commands = new List<IMTLCommandBuffer>();
			foreach (var buffer in request.CommandBuffers)
			{
				if (buffer.IsQueueReady)
				{
					// GENERATE METAL BUFFER
					var cb = buffer.CommandQueue.CommandBuffer();

					AmtCommandRecording recording = GenerateRecording(cb, buffer);

					foreach (var context in buffer.Record.Contexts)
					{
						if (context.Category == AmtEncoderCategory.Compute)
						{
							recording.Compute.Encoder = cb.ComputeCommandEncoder;
						}
						else if (context.Category == AmtEncoderCategory.Blit)
						{
							recording.Blit.Encoder = cb.BlitCommandEncoder;
						}

						for (var i = context.First; i <= context.Last; ++i)
						{
							buffer.Record.Instructions[i].Perform(recording);
						}

						if (context.Category == AmtEncoderCategory.Compute)
						{
							recording.Compute.Encoder.EndEncoding();
							recording.Compute.Encoder = null;
						}
						else if (context.Category == AmtEncoderCategory.Blit)
						{
							recording.Blit.Encoder.EndEncoding();
							recording.Blit.Encoder = null;
						}
					}

					TriggerSemaphores(request.Signals, cb);
					SignalFenceWhenCompleted(request.OrderFence, cb);

					cb.Commit();
					commands.Add(cb);
				}
			}
			return commands.ToArray();
		}

		static void SignalFenceWhenCompleted(AmtSemaphore fence, IMTLCommandBuffer cb)
		{
			// SIGNAL ORDER.FENCE
			if (fence != null)
			{
				fence.Reset();

				cb.AddCompletedHandler(
					(b) =>
					{
						fence.Signal();
					}
				);
			}
		}

		static void TriggerSemaphores(AmtSemaphore[] signals, IMTLCommandBuffer cb)
		{
			// SIGNAL SEMAPHORE
			foreach (var signal in signals)
			{
				signal.Reset();
				// APPLY HANDLER TO COMMAND BUFFER

				cb.AddCompletedHandler(
					(b) =>
					{
						signal.Signal();
					}
				);
			}
		}

		static AmtCommandRecording GenerateRecording(IMTLCommandBuffer cb, AmtCommandBuffer buffer)
		{
			return new AmtCommandRecording
			{
				CommandBuffer = cb,
				Compute = new AmtComputeRecording
				{
					Grid = buffer.Record.ComputeGrid,
				},
				Graphics = new AmtGraphicsRecording
				{
					Grid = buffer.Record.GraphicsGrid,
				},
				Blit = new AmtBlitRecording
				{
					Grid = buffer.Record.BlitGrid,
				},
			};
		}
	}
}
