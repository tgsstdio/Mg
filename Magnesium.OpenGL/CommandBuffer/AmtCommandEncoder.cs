using System;
namespace Magnesium.OpenGL
{
	public class AmtCommandEncoder
	{
		private readonly IAmtEncoderContextSorter mInstructions;

		public AmtCommandEncoder(
			IAmtEncoderContextSorter instructions,
			IAmtGraphicsEncoder graphics,
			IAmtComputeEncoder compute, 
			IAmtBlitEncoder blit
		)
		{
			mInstructions = instructions;
			Graphics = graphics;
			Compute = compute;
			Blit = blit;
		}
		public IAmtGraphicsEncoder Graphics { get; private set; }
		public IAmtComputeEncoder Compute { get; private set;}

		public IAmtBlitEncoder Blit
		{
			get;
			private set;
		}

		public void Clear()
		{
			Graphics.Clear();
			Compute.Clear();
			Blit.Clear();
			mInstructions.Clear();
		}

		public AmtCommandBufferRecord AsRecord()
		{
			var replay = mInstructions.ToReplay();
			replay.ComputeGrid = Compute.AsGrid();
			replay.GraphicsGrid = Graphics.AsGrid();
			replay.BlitGrid = Blit.AsGrid();
			return replay;
		}
	}
}
