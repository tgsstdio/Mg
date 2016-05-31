using System;

namespace Magnesium
{
    public class MgPresentInfoKHR
	{
		public UInt32 WaitSemaphoreCount { get; set; }
		public IMgSemaphore[] WaitSemaphores { get; set; }
		public UInt32 SwapchainCount { get; set; }
		public IMgSwapchainKHR[] Swapchains { get; set; }
		public UInt32[] ImageIndices { get; set; }
		public Result[] Results { get; set; }
	}
}

