using System;
namespace Magnesium.Metal
{
	public class AmtCommandEncoder :IAmtCommandEncoder
	{
		public AmtCommandEncoder(IAmtGraphicsEncoder graphics, IAmtComputeEncoder compute)
		{
			Graphics = graphics;
			Compute = compute;
		}
		public IAmtGraphicsEncoder Graphics { get; private set; }
		public IAmtComputeEncoder Compute { get; private set;}

		public void Clear()
		{
			Graphics.Clear();
			Compute.Clear();
		}
	}
}
