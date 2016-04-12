﻿using System;

namespace Magnesium
{
    [Flags] 
	public enum MgQueryPipelineStatisticFlagBits : ushort
	{
		INPUT_ASSEMBLY_VERTICES_BIT = 1 << 0,
		INPUT_ASSEMBLY_PRIMITIVES_BIT = 1 << 1,
		VERTEX_SHADER_INVOCATIONS_BIT = 1 << 2,
		GEOMETRY_SHADER_INVOCATIONS_BIT = 1 << 3,
		GEOMETRY_SHADER_PRIMITIVES_BIT = 1 << 4,
		CLIPPING_INVOCATIONS_BIT = 1 << 5,
		CLIPPING_PRIMITIVES_BIT = 1 << 6,
		FRAGMENT_SHADER_INVOCATIONS_BIT = 1 << 7,
		TESSELLATION_CONTROL_SHADER_PATCHES_BIT = 1 << 8,
		TESSELLATION_EVALUATION_SHADER_INVOCATIONS_BIT = 1 << 9,
		COMPUTE_SHADER_INVOCATIONS_BIT = 1 << 10,
	}
}

