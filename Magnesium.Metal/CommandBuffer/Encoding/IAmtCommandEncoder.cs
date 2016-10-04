namespace Magnesium.Metal
{
	public interface IAmtCommandEncoder
	{
		IAmtGraphicsEncoder Graphics { get; }
		IAmtComputeEncoder Compute { get; }
		IAmtBlitEncoder Blit { get; }

		void Clear();
		AmtCommandBufferRecord AsRecord();
	}
}