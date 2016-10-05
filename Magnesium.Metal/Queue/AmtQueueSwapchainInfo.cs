namespace Magnesium.Metal
{
	public class AmtQueueSwapchainInfo
	{
		public uint ImageIndex { get; internal set; }
		public IAmtSwapchainKHR Swapchain { get; internal set; }
	}
}