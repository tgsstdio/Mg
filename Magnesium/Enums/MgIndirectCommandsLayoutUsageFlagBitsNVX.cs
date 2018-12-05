using System;

namespace Magnesium
{
	[Flags]
	public enum MgIndirectCommandsLayoutUsageFlagBitsNVX : UInt32
	{
		UNORDERED_SEQUENCES_BIT_NVX = 0x1,
		SPARSE_SEQUENCES_BIT_NVX = 0x2,
		EMPTY_EXECUTIONS_BIT_NVX = 0x4,
		INDEXED_SEQUENCES_BIT_NVX = 0x8,
	}
}
