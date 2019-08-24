using System;

namespace Magnesium
{
	[Flags]
	public enum MgExternalMemoryFeatureFlagBits : UInt32
	{
		DEDICATED_ONLY_BIT = 0x1,
		EXPORTABLE_BIT = 0x2,
		IMPORTABLE_BIT = 0x4,
		DEDICATED_ONLY_BIT_KHR = DEDICATED_ONLY_BIT,
		EXPORTABLE_BIT_KHR = EXPORTABLE_BIT,
		IMPORTABLE_BIT_KHR = IMPORTABLE_BIT,
	}
}
