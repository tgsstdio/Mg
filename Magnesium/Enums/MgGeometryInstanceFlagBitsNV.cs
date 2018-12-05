using System;

namespace Magnesium
{
	[Flags]
	public enum MgGeometryInstanceFlagBitsNV : UInt32
	{
		TRIANGLE_CULL_DISABLE_BIT_NV = 0x1,
		TRIANGLE_FRONT_COUNTERCLOCKWISE_BIT_NV = 0x2,
		FORCE_OPAQUE_BIT_NV = 0x4,
		FORCE_NO_OPAQUE_BIT_NV = 0x8,
	}
}
