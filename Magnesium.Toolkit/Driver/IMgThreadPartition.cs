using System;

namespace Magnesium.Toolkit
{
	public interface IMgThreadPartition : IDisposable
	{
		IMgCommandPool CommandPool { get; }
		IMgCommandBuffer[] CommandBuffers { get; }
		IMgQueue Queue { get; }
		IMgDevice Device { get; }
		IMgPhysicalDevice PhysicalDevice { get; }

		// UTILITY FUNCTION
		bool GetMemoryType(uint typeBits, MgMemoryPropertyFlagBits memoryPropertyFlags, out uint typeIndex);
	}
}

