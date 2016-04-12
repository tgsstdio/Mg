using System;

namespace Magnesium
{
    [Flags] 
	public enum MgImageAspectFlagBits : byte
	{
		COLOR_BIT = 1 << 0,
		DEPTH_BIT = 1 << 1,
		STENCIL_BIT = 1 << 2,
		METADATA_BIT = 1 << 3,
	}
}

