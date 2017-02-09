using System;
namespace Magnesium.OpenGL.Internals
{
	public class GLCmdCommandEncoder
	{
		private readonly IGLCmdEncoderContextSorter mInstructions;

		public GLCmdCommandEncoder(
			IGLCmdEncoderContextSorter instructions,
			IGLCmdGraphicsEncoder graphics,
			IGLCmdComputeEncoder compute, 
			IGLCmdBlitEncoder blit
		)
		{
			mInstructions = instructions;
			Graphics = graphics;
			Compute = compute;
			Blit = blit;
		}
		public IGLCmdGraphicsEncoder Graphics { get; private set; }
		public IGLCmdComputeEncoder Compute { get; private set;}

		public IGLCmdBlitEncoder Blit
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

		public GLCmdCommandBufferRecord AsRecord()
		{
			var replay = mInstructions.ToReplay();
			replay.ComputeGrid = Compute.AsGrid();
			replay.GraphicsGrid = Graphics.AsGrid();
			replay.BlitGrid = Blit.AsGrid();
			return replay;
		}
	}
}
