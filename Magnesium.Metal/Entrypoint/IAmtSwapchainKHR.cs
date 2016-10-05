namespace Magnesium.Metal
{
	public interface IAmtSwapchainKHR : IMgSwapchainKHR
	{
		bool AcquireNextImage(ulong timeout, out uint index);
		AmtSwapchainKHRImageInfo[] Images { get; }
	}
}