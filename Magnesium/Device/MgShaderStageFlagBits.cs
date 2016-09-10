using System;

namespace Magnesium
{
    [Flags] 
	public enum MgShaderStageFlagBits : UInt32
	{
		VERTEX_BIT = 1 << 0,
		TESSELLATION_CONTROL_BIT = 1 << 1,
		TESSELLATION_EVALUATION_BIT = 1 << 2,
		GEOMETRY_BIT = 1 << 3,
		FRAGMENT_BIT = 1 << 4,
		COMPUTE_BIT = 1 << 5,
		ALL_GRAPHICS = 0x1F,
		ALL = 0x7FFFFFFF,
	}
}

