using System;

namespace Magnesium
{
    [Flags] 
	public enum MgBufferUsageFlagBits : ushort
	{
		// Can be used as a source of transfer operations
		TRANSFER_SRC_BIT = 1 << 0,
		// Can be used as a destination of transfer operations
		TRANSFER_DST_BIT = 1 << 1,
		// Can be used as TBO
		UNIFORM_TEXEL_BUFFER_BIT = 1 << 2,
		// Can be used as IBO
		STORAGE_TEXEL_BUFFER_BIT = 1 << 3,
		// Can be used as UBO
		UNIFORM_BUFFER_BIT = 1 << 4,
		// Can be used as SSBO
		STORAGE_BUFFER_BIT = 1 << 5,
		// Can be used as source of fixed-function index fetch (index buffer)
		INDEX_BUFFER_BIT = 1 << 6,
		// Can be used as source of fixed-function vertex fetch (VBO)
		VERTEX_BUFFER_BIT = 1 << 7,
		// Can be the source of indirect parameters (e.g. indirect buffer, parameter buffer)
		INDIRECT_BUFFER_BIT = 1 << 8,
	}
}

