using System;

namespace Magnesium
{
	[Flags]
	public enum MgDescriptorBindingFlagBitsExt : UInt32
	{
		UPDATE_AFTER_BIND_BIT_EXT = 0x1,
		UPDATE_UNUSED_WHILE_PENDING_BIT_EXT = 0x2,
		PARTIALLY_BOUND_BIT_EXT = 0x4,
		VARIABLE_DESCRIPTOR_COUNT_BIT_EXT = 0x8,
	}
}
