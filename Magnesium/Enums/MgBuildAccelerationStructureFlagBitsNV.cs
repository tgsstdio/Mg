using System;

namespace Magnesium
{
	[Flags]
	public enum MgBuildAccelerationStructureFlagBitsNV : UInt32
	{
		ALLOW_UPDATE_BIT_NV = 0x1,
		ALLOW_COMPACTION_BIT_NV = 0x2,
		PREFER_FAST_TRACE_BIT_NV = 0x4,
		PREFER_FAST_BUILD_BIT_NV = 0x8,
		LOW_MEMORY_BIT_NV = 0x10,
	}
}
