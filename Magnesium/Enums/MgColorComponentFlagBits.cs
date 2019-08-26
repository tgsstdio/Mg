using System;

namespace Magnesium
{
	[Flags]
	public enum MgColorComponentFlagBits : byte
	{
		R_BIT = 0x1,
		G_BIT = 0x2,
		B_BIT = 0x4,
		A_BIT = 0x8,
        ALL_BITS = R_BIT | G_BIT | B_BIT | A_BIT,
	}
}

