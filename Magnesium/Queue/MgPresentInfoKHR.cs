using System;

namespace Magnesium
{
    public class MgPresentInfoKHR
	{
		public IMgSemaphore[] WaitSemaphores { get; set; }

		public MgPresentInfoKHRImage[] Images { get; set;}

		public Result[] Results { get; set; }
	}
}

