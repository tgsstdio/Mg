namespace Magnesium.Metal
{
	public interface IAmtSwapchainKHR : IMgSwapchainKHR
	{
		AmtSwapchainKHRImageInfo[] Images { get; }
	}
}