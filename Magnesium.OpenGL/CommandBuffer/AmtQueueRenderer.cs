using System;

namespace Magnesium.OpenGL
{
	public class AmtQueueRenderer
	{
        private readonly IGLQueueRenderer mRenderer;
        private readonly IGLBlitOperationEntrypoint mBlit;
        public AmtQueueRenderer(IGLQueueRenderer renderer, IGLBlitOperationEntrypoint blit)
		{
            mRenderer = renderer;
            mBlit = blit;
		}

		public void Render(IGLCommandBuffer buffer)
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
                        recording.Blit.Entrypoint = mBlit;
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
					////else if (context.Category == AmtEncoderCategory.Blit)
					////{
					////	recording.Blit.Encoder.EndEncoding();
					////	recording.Blit.Encoder = null;
					////}
				}       

			}			
        }

		static AmtCommandRecording GenerateRecording(IGLCommandBuffer buffer, IGLQueueRenderer renderer)
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
