using Magnesium.OpenGL.Internals;
using System;

namespace Magnesium.OpenGL
{
	public class AmtQueueRenderer : IGLQueueRenderer
	{
        private readonly IGLBlitOperationEntrypoint mBlit;
        private readonly AmtStateRenderer mRenderer;
        public AmtQueueRenderer
        (
            IGLBlitOperationEntrypoint blit,

            // for hiding implementation details of state renderer
            IGLCmdBlendEntrypoint blend,
            IGLCmdStencilEntrypoint stencil,
            IGLCmdRasterizationEntrypoint raster,
            IGLCmdDepthEntrypoint depth,
            IGLCmdShaderProgramCache cache,
            IGLCmdScissorsEntrypoint scissor,
            IGLCmdDrawEntrypoint render,
            IGLCmdClearEntrypoint clear,
            IGLErrorHandler errHandler
        )
		{
            mBlit = blit;

            // for hiding implementation details of state renderer
            mRenderer = new AmtStateRenderer
            (
                blend
                ,stencil
                ,raster
                ,depth
                ,cache
                ,scissor
                ,render
                ,clear
                ,errHandler
            );

        }

        public void Initialize()
        {
            mRenderer.Initialize();
        }

        #region Render methods

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

		static AmtCommandRecording GenerateRecording(IGLCommandBuffer buffer, AmtStateRenderer renderer)
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

        #endregion
    }

}
