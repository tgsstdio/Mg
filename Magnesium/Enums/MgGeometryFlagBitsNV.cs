using System;

namespace Magnesium
{
	[Flags]
	public enum MgGeometryFlagBitsNV : UInt32
	{
		GEOMETRY_OPAQUE_BIT_NV = 0x1,
		GEOMETRY_NO_DUPLICATE_ANY_HIT_INVOCATION_BIT_NV = 0x2,
	}
}
