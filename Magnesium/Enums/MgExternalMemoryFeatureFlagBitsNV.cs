using System;

namespace Magnesium
{
	[Flags]
	public enum MgExternalMemoryFeatureFlagBitsNV : UInt32
	{
		DEDICATED_ONLY_BIT_NV = 0x1,
		EXPORTABLE_BIT_NV = 0x2,
		IMPORTABLE_BIT_NV = 0x4,
	}
}
