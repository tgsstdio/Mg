using System;

namespace Magnesium.OpenGL
{
	class AmtQueueRenderer
	{
        private IAmtStateRenderer mRenderer;

        public AmtQueueRenderer(IAmtStateRenderer renderer)
		{
            mRenderer = renderer;
		}

		public void Render(AmtQueueSubmission request)
		{
			foreach (var buffer in request.CommandBuffers)
			{
				if (buffer.IsQueueReady)
				{
					AmtCommandRecording recording = GenerateRecording(buffer, mRenderer);

					foreach (var context in buffer.Record.Contexts)
					{
						if (context.Category == AmtEncoderCategory.Compute)
						{
                            recording.Compute.Encoder = new AmtComputeEncoder();
						}
						else if (context.Category == AmtEncoderCategory.Blit)
						{
							recording.Blit.Encoder = new AmtBlitEncoder();
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

					TriggerSemaphores(request.Signals);

                    if (request.OrderFence != null)
                    {
                        request.OrderFence.Signal();
                    }              

				}
			}
            if (request.Fence != null)
            {
                request.Fence.BeginSync();
            }
        }

		static void TriggerSemaphores(IGLSemaphore[] signals)
		{
			// SIGNAL SEMAPHORE
			foreach (var signal in signals)
			{
				signal.Reset();
				// APPLY HANDLER TO COMMAND BUFFER

				signal.BeginSync();
			}
		}

		static AmtCommandRecording GenerateRecording(AmtCommandBuffer buffer, IAmtStateRenderer renderer)
		{
			return new AmtCommandRecording
			{
				Compute = new AmtComputeRecording
				{
					Grid = buffer.Record.ComputeGrid,
				},
				Graphics = new AmtGraphicsRecording
				{
                    StateRenderer = renderer,
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
