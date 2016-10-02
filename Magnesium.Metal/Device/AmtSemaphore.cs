using System;
using System.Threading;

namespace Magnesium.Metal
{
	public class AmtSemaphore
	{
		public bool IsSignalled { get; private set; } 

		public AmtSemaphore()
		{
			IsSignalled = false;
		}

		internal void Reset()
		{
			IsSignalled = false;
		}

		internal void Signal()
		{
			IsSignalled = true;
		}
	}
}
