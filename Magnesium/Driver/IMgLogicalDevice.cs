using System;

namespace Magnesium
{
	public interface IMgLogicalDevice : IDisposable
	{
		IMgPhysicalDevice GPU { get;  }
		IMgDevice Device { get;  }
		IMgQueueInfo[] Queues { get; }
	}
}

