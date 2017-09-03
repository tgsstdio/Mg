﻿using System;

namespace Magnesium
{
	public interface IMgThreadPartition : IDisposable
	{
		IMgCommandPool CommandPool { get; }
		IMgQueue Queue { get; }
		IMgDevice Device { get; }
		IMgPhysicalDevice PhysicalDevice { get; }
	}
}

