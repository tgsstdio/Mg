using System;

namespace Magnesium
{
	public class MgAcquireNextImageInfoKHR
	{
		public IMgSwapchainKHR Swapchain { get; set; }
		public UInt64 Timeout { get; set; }
		public IMgSemaphore Semaphore { get; set; }
		public IMgFence Fence { get; set; }
		public UInt32 DeviceMask { get; set; }
	}
}
