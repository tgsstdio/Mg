using System;

namespace Magnesium
{
    [Flags] 
	public enum MgQueueFlagBits : byte
	{
		// Queue supports graphics operations
		GRAPHICS_BIT = 1 << 0,
		// Queue supports compute operations
		COMPUTE_BIT = 1 << 1,
		// Queue supports transfer operations
		TRANSFER_BIT = 1 << 2,
		// Queue supports sparse resource memory management operations
		SPARSE_BINDING_BIT = 1 << 3,
	}
}

