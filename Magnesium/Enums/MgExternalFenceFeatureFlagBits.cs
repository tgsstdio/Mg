using System;

namespace Magnesium
{
	[Flags]
	public enum MgExternalFenceFeatureFlagBits : UInt32
	{
		EXPORTABLE_BIT = 0x1,
		IMPORTABLE_BIT = 0x2,
		EXPORTABLE_BIT_KHR = EXPORTABLE_BIT,
		IMPORTABLE_BIT_KHR = IMPORTABLE_BIT,
	}
}
