using System;
namespace Magnesium.Metal
{
	public class AmtCommandEncoder :IAmtCommandEncoder
	{
		private readonly IAmtEncoderContextSorter mInstructions;

		public AmtCommandEncoder(IAmtEncoderContextSorter instructions, IAmtGraphicsEncoder graphics, IAmtComputeEncoder compute)
		{
			mInstructions = instructions;
			Graphics = graphics;
			Compute = compute;
		}
		public IAmtGraphicsEncoder Graphics { get; private set; }
		public IAmtComputeEncoder Compute { get; private set;}

		public void Clear()
		{
			Graphics.Clear();
			Compute.Clear();
			mInstructions.Clear();
		}

		public AmtCommandBufferRecord AsRecord()
		{
			var replay = mInstructions.ToReplay();
			replay.ComputeGrid = Compute.AsGrid();
			replay.GraphicsGrid = Graphics.AsGrid();
			return replay;
		}
	}
}
