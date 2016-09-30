namespace Magnesium.Metal
{
	public interface IAmtCommandEncoder
	{
		IAmtGraphicsEncoder Graphics { get; }
		IAmtComputeEncoder Compute { get; }

		void Clear();

	}
}