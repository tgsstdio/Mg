using System;

namespace Magnesium
{
	public class MgSemaphoreGetFdInfoKHR
	{
		public IMgSemaphore Semaphore { get; set; }
		public MgExternalSemaphoreHandleTypeFlagBits HandleType { get; set; }
	}
}
