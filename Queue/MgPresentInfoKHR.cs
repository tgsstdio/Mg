using System;

namespace Magnesium
{
    public class MgPresentInfoKHR
	{
		public UInt32 WaitSemaphoreCount { get; set; }
		public MgSemaphore[] WaitSemaphores { get; set; }
		public UInt32 SwapchainCount { get; set; }
		public MgSwapchainKHR[] Swapchains { get; set; }
		public UInt32[] ImageIndices { get; set; }
		public Result[] Results { get; set; }
	}
}

