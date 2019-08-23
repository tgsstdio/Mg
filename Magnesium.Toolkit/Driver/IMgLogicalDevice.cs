using System;

namespace Magnesium.Toolkit
{
	public interface IMgLogicalDevice : IDisposable
	{
		IMgPhysicalDevice GPU { get;  }
		IMgDevice Device { get;  }
		IMgQueueInfo[] Queues { get; }
	}
}

