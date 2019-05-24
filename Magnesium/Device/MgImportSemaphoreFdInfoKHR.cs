using System;

namespace Magnesium
{
	public class MgImportSemaphoreFdInfoKHR
	{
		public IMgSemaphore Semaphore { get; set; }
		public MgSemaphoreImportFlagBits Flags { get; set; }
		public MgExternalSemaphoreHandleTypeFlagBits HandleType { get; set; }
		public int Fd { get; set; }
	}
}
