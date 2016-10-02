using System;

namespace Magnesium.Metal
{
	public class AmtSemaphoreEntrypoint : IAmtSemaphoreEntrypoint
	{
		public AmtSemaphore CreateSemaphore()
		{
			return new AmtSemaphore();
		}
	}
}