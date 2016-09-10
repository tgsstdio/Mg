using System;

namespace Magnesium
{
    [Flags] 
	public enum MgPipelineCreateFlagBits : byte
	{
		DISABLE_OPTIMIZATION_BIT = 1 << 0,
		ALLOW_DERIVATIVES_BIT = 1 << 1,
		DERIVATIVE_BIT = 1 << 2,
	}
}

